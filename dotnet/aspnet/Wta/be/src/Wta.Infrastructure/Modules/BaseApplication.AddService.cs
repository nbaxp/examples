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
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wta.Infrastructure.Modules;

public abstract partial class BaseApplication
{
    public virtual void AddServiceProviderFactory(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
        {
            containerBuilder.RegisterModule(new ConfigurationModule(builder.Configuration));
        }));
    }

    public virtual void AddLogging(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((hostingContext, configBuilder) =>
        {
            configBuilder.ReadFrom.Configuration(hostingContext.Configuration)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext();
        });
    }

    public virtual void AddMonitoring(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().ForwardToPrometheus();
        builder.Services.AddSystemMetrics();
    }

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

    public virtual void AddContentProvider(WebApplicationBuilder builder)
    {
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings.Add(".apk", "application/vnd.android.package-archive");
        contentTypeProvider.Mappings.Add(".ipa", "application/vnd.iphone");
        builder.Services.AddSingleton(contentTypeProvider);
    }

    public virtual void AddCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(Constants.CORS_POLICY, builder =>
            {
                builder.SetIsOriginAllowed(isOriginAllowed => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
    }

    public virtual void AddCache(WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("Redis");
            options.InstanceName = "wta";
        });
    }

    public virtual void AddRouting(WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options =>
        {
            options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
        });
    }

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
        builder.Services.AddLocalization();
        builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Resources")
            .AddDataAnnotationsPortableObjectLocalization();
        builder.Services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, LocalizationFileLocationProvider>());
    }

    public virtual void AddMvc(WebApplicationBuilder builder)
    {
        static void configJson(JsonSerializerOptions options)
        {
            options.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.DefaultBufferSize *= 10;
            options.WriteIndented = false;// builder.Environment.IsDevelopment();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(new JsonNullableGuidConverter());
            options.Converters.Insert(0, new TrimJsonConverter());
            options.NumberHandling= JsonNumberHandling.AllowNamedFloatingPointLiterals;
        }
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

    public virtual void AddJsonOptions(WebApplicationBuilder builder)
    {
        builder.Services.AddDateOnlyTimeOnlyStringConverters();
        builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<Microsoft.AspNetCore.Http.Json.JsonOptions>>().Value.SerializerOptions);
    }

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

    public virtual void AddDistributedLock(WebApplicationBuilder builder)
    {
        //var redis = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);
        //builder.Services.AddSingleton<IDistributedLockProvider>(o => new RedisDistributedLock(redis));
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
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.IsAssignableTo(typeof(ITenant)) && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)))
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
    }

    public virtual void AddRepository(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
    }

    public virtual void AddProfile(WebApplicationBuilder builder)
    {
        builder.Services.AddMiniProfiler(o => o.RouteBasePath = "/profiler").AddEntityFramework();
    }

    public virtual void AddFileProvider(WebApplicationBuilder builder)
    {
        var providers = AppDomain.CurrentDomain.GetCustomerAssemblies()
            .Select(o => new FixedEmbeddedFileProvider(o))
            .ToArray()
            .Append(builder.Environment.ContentRootFileProvider);

        if (Directory.Exists(builder.Environment.WebRootPath))
        {
            //providers = providers.Append(new ManifestEmbeddedFileProvider(Assembly.GetEntryAssembly()!, builder.Environment.WebRootPath));
            providers = providers.Append(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), builder.Environment.WebRootPath)));
        }
        builder.Services.AddSingleton<IFileProvider>(new CompositeFileProvider(providers));
    }
}
