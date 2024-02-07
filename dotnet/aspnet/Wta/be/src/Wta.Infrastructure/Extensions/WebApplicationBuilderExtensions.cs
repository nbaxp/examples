using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrchardCore.Localization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Common;
using Wta.Infrastructure.Configuration;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Resources;
using Wta.Infrastructure.Services;
using Wta.Infrastructure.Web;
using Wta.Shared;
using Wta.Shared.Data;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Wta.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{    /// <summary>
     /// 添加服务
     /// </summary>
     /// <param name="builder"></param>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        //使用 Autofac 取代内置的依赖注入框架
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        // 添加默认服务
        builder.AddDefaultServices();
        // 添加默认配置
        builder.AddDefaultOptions();
        // HTTP 相关配置
        builder.AddDefaultHttp();
        builder.Services.AddRouting(options =>
        {
            options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
        });
        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.Mappings.Add(".apk", "application/vnd.android.package-archive");
        contentTypeProvider.Mappings.Add(".ipa", "application/vnd.iphone");
        builder.Services.AddSingleton(contentTypeProvider);
        builder.AddDefaultLocalization();
        builder.AddDefaultMvc();
        builder.AddDefaultSwager();
        builder.AddDefaultAuth();
        builder.AddDefaultDbContext();
        builder.AddObjectMapper();
    }

    public static void AddObjectMapper(this WebApplicationBuilder builder)
    {
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IMapperConfig<,>)))
            .ForEach(type =>
            {
                var config = Activator.CreateInstance(type);
                type.GetInterfaces().Where(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IMapperConfig<,>))
                .ForEach(o =>
                {
                    var builderType = typeof(MapperConfigBuilder<,>).MakeGenericType(o.GetGenericArguments());
                    builder.Services.AddTransient(o, type);
                });
            });
    }

    /// <summary>
    /// 添加默认服务
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultServices(this WebApplicationBuilder builder)
    {
        WebApp.Instance.Assemblies.SelectMany(o => o.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(ServiceAttribute<>)).Any())
            .ForEach(type =>
            {
                type.GetCustomAttributes(typeof(ServiceAttribute<>)).Select(o => (o as IImplementAttribute)!).ForEach(attribute =>
                {
                    Platform currentPlatformType;
                    if (OperatingSystem.IsWindows())
                    {
                        currentPlatformType = Platform.Windows;
                    }
                    else if (OperatingSystem.IsLinux())
                    {
                        currentPlatformType = Platform.Linux;
                    }
                    else if (OperatingSystem.IsFreeBSD())
                    {
                        currentPlatformType = Platform.FreeBSD;
                    }
                    else
                    {
                        currentPlatformType = Platform.OSX;
                    }
                    if (attribute.Platform.HasFlag(currentPlatformType))
                    {
                        if (attribute.ServiceType.IsAssignableTo(typeof(IHostedService)))
                        {
                            var method = typeof(ServiceCollectionHostedServiceExtensions)
                            .GetMethod(nameof(ServiceCollectionHostedServiceExtensions.AddHostedService),
                            new[] { typeof(IServiceCollection) });
                            method?.MakeGenericMethod(type).Invoke(null, new object[] { builder.Services });
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
                    }
                });
            });
    }

    /// <summary>
    /// 添加配置项
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultOptions(this WebApplicationBuilder builder)
    {
        WebApp.Instance.Assemblies.SelectMany(o => o.GetTypes()).Where(type => type.GetCustomAttributes<OptionsAttribute>().Any()).ForEach(type =>
        {
            var attribute = type.GetCustomAttribute<OptionsAttribute>()!;
            var configurationSection = builder.Configuration.GetSection(attribute.Section ?? type.Name.TrimEnd("Options"));
            typeof(OptionsConfigurationServiceCollectionExtensions).InvokeExtensionMethod("Configure", [type], [typeof(IConfiguration)], builder.Services, configurationSection);
        });
    }

    /// <summary>
    /// 添加数据上下文服务
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        //添加实体配置
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDbConfig<>)))
            .ForEach(o =>
            {
                var dbContextType = o.GetInterfaces().First(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IDbConfig<>)).GetGenericArguments()[0];
                builder.Services.AddScoped(typeof(IDbConfig<>).MakeGenericType(dbContextType), o);
            });
        //添加种子服务
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDbSeeder<>)))
            .ForEach(o =>
            {
                var dbContextType = o.GetInterfaces().First(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(IDbSeeder<>)).GetGenericArguments()[0];
                builder.Services.AddScoped(typeof(IDbSeeder<>).MakeGenericType(dbContextType), o);
            });
        //添加数据上下文服务
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
            .ForEach(dbContextType =>
            {
                void optionsAction(DbContextOptionsBuilder optionsBuilder)
                {
                    var connectionStringName = dbContextType.GetCustomAttribute<ConnectionStringAttribute>()?.ConnectionString ?? dbContextType.Name.TrimEnd("DbContext");
                    var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ??
                         builder.Configuration.GetConnectionString($"Default") ??
                         "Data Source=data.db";
                    var dbContextProvider = builder.Configuration.GetValue<string>($"DbContext:{connectionStringName}") ??
                        builder.Configuration.GetValue<string>($"DbContext:Default") ??
                        "Sqlite";
                    if (dbContextProvider == "MySql")
                    {
                        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                    }
                    else if (dbContextProvider == "Npgsql")
                    {
                        optionsBuilder.UseNpgsql(connectionString);
                    }
                    else if (dbContextProvider == "SqlServer")
                    {
                        optionsBuilder.UseSqlServer(connectionString);
                    }
                    else if (dbContextProvider == "Oracle")
                    {
                        optionsBuilder.UseOracle(connectionString);
                    }
                    else
                    {
                        optionsBuilder.UseSqlite(connectionString);
                    }
                }
                var optionsType = typeof(DbContextOptions<>).MakeGenericType(dbContextType);
                builder.Services.AddDbContext(dbContextType, optionsAction);
            });
    }

    /// <summary>
    /// HTTP 配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultHttp(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.SetIsOriginAllowed(isOriginAllowed => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
        });
    }

    /// <summary>
    /// 添加资源文件配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultLocalization(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(new CustomFileProvider(Assembly.GetExecutingAssembly(), $"{nameof(Wta.Infrastructure)}.wwwroot"));
        builder.Services.AddSingleton<IModelMetadataProvider, CustomModelMetaDataProvider>();
        builder.Services.AddTransient<IStringLocalizer>(o => o.GetRequiredService<IStringLocalizer<Resource>>());
        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("zh-CN"),
                new CultureInfo("en-US"),
            };
            options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
        builder.Services.AddSingleton<IFileProvider>(new CompositeFileProvider(builder.Environment.ContentRootFileProvider, new EmbeddedFileProvider(Assembly.GetExecutingAssembly())));
        builder.Services.AddLocalization();
        builder.Services.AddPortableObjectLocalization(options => options.ResourcesPath = "Resources")
            .AddDataAnnotationsPortableObjectLocalization();
        builder.Services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, CustomLocalizationFileLocationProvider>());
    }

    /// <summary>
    /// 添加 MVC 配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultMvc(this WebApplicationBuilder builder)
    {
        builder.Services.AddMvc(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            options.ModelBinderProviders.Insert(0, new CustomModelBinderProvider());
            options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            options.ModelMetadataDetailsProviders.Insert(0, new CustomDisplayMetadataProvider());
            options.ModelMetadataDetailsProviders.Add(new CustomValidationMetadataProvider());
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            options.Conventions.Add(new CustomControllerRouteConvention());
            options.Filters.Add<CustomActionFilter>();
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
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            options.JsonSerializerOptions.WriteIndented = builder.Environment.IsDevelopment() ? true : false;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.JsonSerializerOptions.Converters.Add(new CustomJsonNullableGuidConverter());
            options.JsonSerializerOptions.Converters.Insert(0, new CustomJsonTrimConverter());
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .ConfigureApplicationPartManager(o => o.FeatureProviders.Add(new GenericControllerFeatureProvider()))
        .AddControllersAsServices();//配置 controller 使用依赖注入创建
        builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions);
        builder.Services.AddEndpointsApiExplorer();
    }

    /// <summary>
    /// Swagger 配置
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultSwager(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, CustomSwaggerGenOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SchemaFilter<CustomSwaggerFilter>();
            options.OperationFilter<CustomSwaggerFilter>();
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
    /// 认证和授权
    /// </summary>
    /// <param name="builder"></param>
    public static void AddDefaultAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<JsonWebTokenHandler>();
        builder.Services.AddSingleton<CustomJwtSecurityTokenHandler>();
        builder.Services.AddSingleton<JwtSecurityTokenHandler, CustomJwtSecurityTokenHandler>();
        builder.Services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, CustomJwtBearerPostConfigureOptions>();
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
        });
        builder.Services.AddAuthorization();
    }
}
