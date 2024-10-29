using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using OrchardCore.Localization;
using Prometheus;
using Prometheus.SystemMetrics;
using Serilog;
using Serilog.Events;
using StackExchange.Profiling;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Wta.Infrastructure.Locking;
using Wta.Infrastructure.Tenant;

namespace Wta.Infrastructure.Startup;

public abstract class BaseStartup : IStartup
{
    private const string CorsPolicy = nameof(CorsPolicy);

    /// <summary>
    /// 添加认证和授权
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddAuth(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<JsonWebTokenHandler>();
        builder.Services.AddSingleton<AuthJwtSecurityTokenHandler>();
        builder.Services.AddSingleton<JwtSecurityTokenHandler, AuthJwtSecurityTokenHandler>();
        builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, AuthJwtBearerPostConfigureOptions>();
        var jwtOptions = new JwtOptions();
        builder.Configuration.GetSection("Jwt").Bind(jwtOptions);
        builder.Services.AddSingleton(jwtOptions);
        var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey));
        builder.Services.AddSingleton(new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256Signature));
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = issuerSigningKey,
            NameClaimType = nameof(ClaimTypes.Name).ToLowerInvariant(),
            RoleClaimType = nameof(ClaimTypes.Role).ToLowerInvariant(),
            ClockSkew = TimeSpan.FromSeconds(30),
        };
        builder.Services.AddSingleton(tokenValidationParameters);
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.UseSecurityTokenValidators = true;
            options.TokenValidationParameters = tokenValidationParameters;
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken))// && path.StartsWithSegments("/api/hub"))
                    {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
        builder.Services.AddAuthorization();
    }

    /// <summary>
    /// 添加缓存
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddCache(WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "wta";
        });
    }

    /// <summary>
    /// 添加 ContentProvider
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddContentProvider(WebApplicationBuilder builder)
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings.Add(".apk", "application/vnd.android.package-archive");
        contentTypeProvider.Mappings.Add(".ipa", "application/vnd.iphone");
        builder.Services.AddSingleton(contentTypeProvider);
    }

    /// <summary>
    /// 添加CORS
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicy, builder =>
            {
                builder.SetIsOriginAllowed(isOriginAllowed => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
    }

    /// <summary>
    /// 添加 DbContext
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddDbContext(WebApplicationBuilder builder)
    {
        //添加实体配置
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(BaseDbConfig<>)))
            .ForEach(configType =>
            {
                configType.GetInterfaces().Where(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)).ForEach(o =>
                {
                    builder.Services.AddScoped(typeof(IEntityTypeConfiguration<>).MakeGenericType(o.GenericTypeArguments.First()), configType);
                });
            });
        //添加种子服务
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDbSeeder<>)))
            .ForEach(o =>
            {
                var dbContextType = o.GetInterfaces().First(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IDbSeeder<>)).GetGenericArguments()[0];
                builder.Services.AddScoped(typeof(IDbSeeder<>).MakeGenericType(dbContextType), o);
            });
        //添加数据上下文服务
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
            .ForEach(dbContextType =>
            {
                var action = (DbContextOptionsBuilder optionsBuilder) =>
                {
                    var connectionStringName = dbContextType.GetCustomAttribute<ConnectionStringAttribute>()?.ConnectionString ?? dbContextType.Name;
                    var connectionString = builder.Configuration.GetConnectionString(connectionStringName);
                    var dbContextProvider = builder.Configuration.GetValue<string>($"DbContextProviders:{connectionStringName}");
                    var migrationsAssemblyName = "Wta.Migrations";
                    if (dbContextProvider == "mysql")
                    {
                        var serverVersion = ServerVersion.AutoDetect(connectionString);
                        optionsBuilder.UseMySql(connectionString, serverVersion, b =>
                        {
                            b.UseNetTopologySuite();
                            b.MigrationsAssembly(migrationsAssemblyName);
                        });
                    }
                    else if (dbContextProvider == "npgsql")
                    {
                        optionsBuilder.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssemblyName));
                    }
                    else if (dbContextProvider == "sqlserver")
                    {
                        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssemblyName));
                    }
                    else if (dbContextProvider == "oracle")
                    {
                        optionsBuilder.UseOracle(connectionString, b => b.MigrationsAssembly(migrationsAssemblyName));
                    }
                    else
                    {
                        optionsBuilder.UseSqlite(connectionString, b =>
                        {
                            b.UseNetTopologySuite();
                            b.MigrationsAssembly(migrationsAssemblyName);
                        });
                    }
                };
                typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethods()
                .First(o => o.Name == nameof(EntityFrameworkServiceCollectionExtensions.AddDbContext) && o.IsGenericMethod && o.GetGenericArguments().Length == 1 && o.GetParameters().Length == 4)
                .MakeGenericMethod(dbContextType)
                .Invoke(null, [builder.Services, action, ServiceLifetime.Scoped, ServiceLifetime.Scoped]);
            });
    }

    public virtual void AddScheduler(WebApplicationBuilder builder)
    {
        var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
        builder.Services.AddHangfire(o =>
        {
            o.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRedisStorage(redis);
        });
        builder.Services.AddHangfireServer(o =>
        {
            o.SchedulePollingInterval = TimeSpan.FromSeconds(1);
            o.HeartbeatInterval = TimeSpan.FromSeconds(1000);
            o.WorkerCount = Math.Max(Environment.ProcessorCount, 10);
            o.StopTimeout = TimeSpan.FromSeconds(15);
            o.ShutdownTimeout = TimeSpan.FromSeconds(30);
        });
    }

    public virtual void AddProfile(WebApplicationBuilder builder)
    {
        builder.Services.AddMiniProfiler(o => o.RouteBasePath = "/profiler").AddEntityFramework();
    }

    /// <summary>
    /// 添加默认配置
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddDefaultOptions(WebApplicationBuilder builder)
    {
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes()).Where(type => type.GetCustomAttributes<OptionsAttribute>().Any()).ForEach(type =>
            {
                var attribute = type.GetCustomAttribute<OptionsAttribute>()!;
                var configurationSection = builder.Configuration.GetSection(attribute.Section ?? type.Name.TrimEnd("Options"));
                typeof(OptionsConfigurationServiceCollectionExtensions).InvokeExtensionMethod("Configure", [type], [typeof(IConfiguration)], builder.Services, configurationSection);
            });
    }

    /// <summary>
    /// 添加默认服务
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddDefaultServices(WebApplicationBuilder builder)
    {
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(ServiceAttribute<>)).Any())
            .ForEach(type =>
            {
                type.GetCustomAttributes(typeof(ServiceAttribute<>)).Select(o => (o as IImplementAttribute)!).ForEach(attribute =>
                {
                    if (attribute.ServiceType.IsAssignableTo(typeof(IHostedService)))
                    {
                        builder.Services.AddSingleton(type);
                        typeof(WebApplicationBuilderExtensions)
                        .GetMethods()
                        .First(o => o.Name == nameof(WebApplicationBuilderExtensions.AddHostedServiceFromServiceProvider))
                        .MakeGenericMethod(type).Invoke(null, [builder]);
                    }
                    else
                    {
                        if (attribute.Lifetime == ServiceLifetime.Singleton)
                        {
                            builder.Services.TryAddSingleton(attribute.ServiceType, type);
                        }
                        else if (attribute.Lifetime == ServiceLifetime.Scoped)
                        {
                            builder.Services.TryAddScoped(attribute.ServiceType, type);
                        }
                        else
                        {
                            builder.Services.TryAddTransient(attribute.ServiceType, type);
                        }
                    }
                });
            });
    }

    public virtual void AddDistributedLock(WebApplicationBuilder builder)
    {
        //var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
        //builder.Services.AddSingleton<IDistributedLockProvider>(o => new RedisDistributedLock(redis));
    }

    public virtual void AddMonitoring(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().ForwardToPrometheus();
        builder.Services.AddSystemMetrics();
    }

    /// <summary>
    /// 添加HTTP配置
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddHttpServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = long.MaxValue;
        });
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
    }

    /// <summary>
    /// 添加 JsonSerializerOptions
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddJsonOptions(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>().Value.SerializerOptions);
    }

    /// <summary>
    /// 添加本地事件总线
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddLocalEventBus(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventPublisher, LocalEventPublisher>();
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(t => t.GetInterfaces().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IEventHander<>)))
            .ToList()
            .ForEach(type =>
            {
                type.GetInterfaces()
                .Where(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IEventHander<>)).ToList()
                .ForEach(o => builder.Services.AddScoped(o, type));
            });
    }

    /// <summary>
    /// 添加本地化
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddLocalization(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IModelMetadataProvider, DisplayModelMetaDataProvider>();
        builder.Services.AddTransient<IStringLocalizer>(o => o.GetRequiredService<IStringLocalizer<Resource>>());
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("zh-CN"),
                new("en-US"),
            };
            options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        builder.Services.AddSingleton(GetFileProvider(builder));
        builder.Services.AddLocalization();
        builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Resources")
            .AddDataAnnotationsPortableObjectLocalization();
        builder.Services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, LocalizationFileLocationProvider>());
    }

    /// <summary>
    /// 添加日志
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostingContext, configBuilder) =>
        {
            configBuilder.ReadFrom.Configuration(hostingContext.Configuration)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext();
        });
    }

    public virtual void AddMvc(WebApplicationBuilder builder)
    {
        var configJson = (JsonSerializerOptions options) =>
        {
            options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.DefaultBufferSize *= 10;
            options.WriteIndented = false;// builder.Environment.IsDevelopment();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(new JsonNullableGuidConverter());
            options.Converters.Insert(0, new TrimJsonConverter());
        };
        builder.Services.Configure<JsonOptions>(o => configJson(o.SerializerOptions));
        builder.Services.AddMvc(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            options.ModelBinderProviders.Insert(0, new TrimModelBinderProvider());
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            options.ModelMetadataDetailsProviders.Insert(0, new AutoErrorMessageMetadataProvider());
            options.ModelMetadataDetailsProviders.Add(new RequiredValidationMetadataProvider());
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            options.Conventions.Add(new AutoControllerRouteConvention());
            options.Filters.Add<AuthActionFilter>();
        }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) =>
            {
                var localizer = factory.Create(typeof(Resource));
                return localizer;
            };
        })
        .AddJsonOptions(options =>
        {
            configJson(options.JsonSerializerOptions);
            //options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            //options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment();
            //options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            //options.JsonSerializerOptions.Converters.Add(new JsonNullableGuidConverter());
            //options.JsonSerializerOptions.Converters.Insert(0, new TrimJsonConverter());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .ConfigureApplicationPartManager(o => o.FeatureProviders.Add(new GenericControllerFeatureProvider()))
        .AddControllersAsServices();//配置 controller 使用依赖注入创建

        //add urlhelper
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped(o =>
        {
            var actionContext = o.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = o.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext!);
        });
    }

    /// <summary>
    /// 添加 OpenApi
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddOpenApi(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, GroupSwaggerGenOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            options.OperationFilter<LanguageSwaggerFilter>();
            options.DocInclusionPredicate((name, apiDescription) =>
            {
                return name == apiDescription.GroupName;
            });
            var id = "Beare";
            options.AddSecurityDefinition(id, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = id
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    /// <summary>
    /// 添加资源库
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddRepository(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
    }

    /// <summary>
    /// 添加路由
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddRouting(WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options =>
        {
            options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
        });
    }

    /// <summary>
    /// 添加依赖注入
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddServiceProviderFactory(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterModule(new ConfigurationModule(builder.Configuration));
        }));
    }

    /// <summary>
    /// 添加 SignalR
    /// </summary>
    /// <param name="builder"></param>
    public virtual void AddSignalR(WebApplicationBuilder builder)
    {
        var signalRServerBuilder = builder.Services.AddSignalR(o =>
        {
            o.EnableDetailedErrors = true;
        });
        var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            var prefix = Assembly.GetEntryAssembly()!.GetName().Name;
            signalRServerBuilder.AddStackExchangeRedis(redisConnectionString, o =>
            {
                o.Configuration.ChannelPrefix = RedisChannel.Literal("signalr");
            });
        }
    }

    /// <summary>
    /// 2.配置应用程序
    /// https://learn.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0&tabs=aspnetcore2x#middleware-order
    /// </summary>
    /// <param name="webApplication"></param>
    public virtual void Configure(WebApplication webApplication)
    {
        UseMonitoring(webApplication);
        UseLogging(webApplication);
        UseStaticFiles(webApplication);
        UseRouting(webApplication);
        UseOpenApi(webApplication);
        UseAuth(webApplication);
        UseEndpoints(webApplication);
        UseSignalR(webApplication);
        UseCORS(webApplication);
        UseLocalization(webApplication);
        UseDbContext(webApplication);
        UseScheduler(webApplication);
        UseProfile(webApplication);
    }

    public virtual void UseProfile(WebApplication webApplication)
    {
        webApplication.UseMiniProfiler();
    }

    /// <summary>
    /// 1.配置依赖注入
    /// </summary>
    /// <param name="builder"></param>
    public virtual void ConfigureServices(WebApplicationBuilder builder)
    {
        AddMonitoring(builder);
        AddServiceProviderFactory(builder);
        AddLogging(builder);
        AddDefaultOptions(builder);
        AddDefaultServices(builder);
        AddLocalEventBus(builder);
        AddHttpServices(builder);
        AddContentProvider(builder);
        AddCors(builder);
        AddCache(builder);
        AddRepository(builder);
        AddRouting(builder);
        AddSignalR(builder);
        AddLocalization(builder);
        AddMvc(builder);
        AddJsonOptions(builder);
        AddOpenApi(builder);
        AddAuth(builder);
        AddDistributedLock(builder);
        AddDbContext(builder);
        AddScheduler(builder);
        AddProfile(builder);
    }

    public virtual IFileProvider GetFileProvider(WebApplicationBuilder builder)
    {
        var providers = AppDomain.CurrentDomain.GetCustomerAssemblies()
            .Select(o => new FixedEmbeddedFileProvider(o)).ToArray()
            .Append(builder.Environment.ContentRootFileProvider)
            .Append(new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot"))
            .Append(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
        return new CompositeFileProvider(providers);
    }

    /// <summary>
    /// 6.配置认证和授权
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseAuth(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    /// <summary>
    /// 9.配置 CORS
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseCORS(WebApplication app)
    {
        //必须在 MapHub 之后
        app.UseCors(CorsPolicy);
    }

    /// <summary>
    /// 11.配置 DbContext
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseDbContext(WebApplication app)
    {
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
            .OrderBy(o => o.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0)
            .ForEach(dbContextType =>
            {
                using var scope = app.Services.CreateScope();
                var serviceProvider = scope.ServiceProvider;
                var contextName = dbContextType.Name;
                if (serviceProvider.GetRequiredService(dbContextType) is DbContext dbContext)
                {
                    if (dbContext.Database.EnsureCreated())
                    {
                        var @lock = app.Services.GetRequiredService<ILock>();
                        using var handle = @lock.Acquire("seed");
                        if (handle != null)
                        {
                            var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
                            serviceProvider.GetServices(dbSeedType)
                            .OrderBy(o => o!.GetType().GetAttribute<DisplayAttribute>()?.GetOrder() ?? 0)
                            .ForEach(o =>
                            {
                                dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(o, [dbContext]);
                            });
                        }
                    }
                }
            });
    }

    /// <summary>
    /// 7.配置 Endpoints
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseEndpoints(WebApplication app)
    {
        app.MapDefaultControllerRoute();
    }

    /// <summary>
    /// 1.配置健康检查
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseMonitoring(WebApplication app)
    {
        app.MapHealthChecks("/hc");
        app.UseMetricServer("/api/metrics").UseHttpMetrics();
    }

    /// <summary>
    /// 10.配置 Localization
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseLocalization(WebApplication app)
    {
        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
        app.UseRequestLocalization(localizationOptions);
    }

    /// <summary>
    /// 2.配置日志
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseLogging(WebApplication app)
    {
        app.UseSerilogRequestLogging(o =>
        {
            o.MessageTemplate = "[{TenantNumber}:{UserName}] HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            o.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var tenantNumber = httpContext.User.Claims.FirstOrDefault(o => o.Type == "TenantNumber")?.Value ?? TenantConstants.ROOT;
                var userName = httpContext.User.Identity?.Name;
                diagnosticContext.Set("TenantNumber", tenantNumber);
                diagnosticContext.Set("UserName", userName);
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("ContentType", httpContext.Response.ContentType);
            };
        });
    }

    /// <summary>
    /// 5.配置 OpenApi
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseOpenApi(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var apiDescriptionGroups = app.Services.GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;
                foreach (var description in apiDescriptionGroups)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }
    }

    /// <summary>
    /// 3.配置路由
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseRouting(WebApplication app)
    {
        app.UseRouting();
    }

    public static void JobTest()
    {
        Console.WriteLine($"test:{Environment.ProcessId},{DateTime.Now}");
    }

    public virtual void UseScheduler(WebApplication webApplication)
    {
        //https://cron.ciding.cc/
        webApplication.UseHangfireDashboard();

        RecurringJob.AddOrUpdate("job_process_test", () => JobTest(), "*/10 * * * * *");
        //webApplication.Services.UseScheduler(o =>
        //{
        //    webApplication.Services.GetServices<IScheduledTask>().ForEach(t =>
        //    {
        //        try
        //        {
        //            o.Schedule(async () =>
        //            {
        //                var connectionString = webApplication.Configuration.GetConnectionString("redis")!;
        //                var connection = await ConnectionMultiplexer.ConnectAsync(connectionString).ConfigureAwait(false);
        //                var redisLock = new RedisDistributedLock(t.GetType().FullName, connection.GetDatabase());
        //                using var handle = await redisLock.TryAcquireAsync().ConfigureAwait(false);
        //                if (handle != null)
        //                {
        //                    Console.WriteLine("已获取分布式加锁，开始执行");
        //                    await t.ExecuteAsync().ConfigureAwait(false);
        //                }
        //                else
        //                {
        //                    Console.WriteLine("未获取分布式加锁成功，跳过执行");
        //                }
        //            }).Cron(t.Cron);
        //        }
        //        catch (Exception ex)
        //        {
        //            webApplication.Logger.LogError(ex.ToString());
        //        }
        //    });
        //});
    }

    /// <summary>
    /// 8.配置 SignalR
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseSignalR(WebApplication app)
    {
        app.MapHub<DefaultHub>("/api/hub");
    }

    /// <summary>
    /// 4.配置静态文件
    /// </summary>
    /// <param name="app"></param>
    public virtual void UseStaticFiles(WebApplication app)
    {
        //app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = app.Services.GetRequiredService<IFileProvider>(),
            //FileProvider = new CompositeFileProvider(app.Services.GetRequiredService<CustomFileProvider>(),
            //    new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot"),
            //    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))),
            ContentTypeProvider = app.Services.GetRequiredService<FileExtensionContentTypeProvider>(),
            ServeUnknownFileTypes = true,
        });
    }
}
