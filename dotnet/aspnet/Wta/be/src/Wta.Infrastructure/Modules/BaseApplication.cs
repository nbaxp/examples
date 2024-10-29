using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;
using Serilog;
using Serilog.Events;

namespace Wta.Infrastructure.Modules;

public abstract partial class BaseApplication : IApplication
{
    public virtual void ConfigureServices(WebApplicationBuilder builder)
    {
        AddServiceProviderFactory(builder);
        AddLogging(builder);
        AddMonitoring(builder);
        AddDefaultOptions(builder);
        AddDefaultServices(builder);
        //AddLocalEventBus(builder);
        //AddHttpServices(builder);
        //AddContentProvider(builder);
        //AddCors(builder);
        //AddCache(builder);
        //AddRepository(builder);
        //AddRouting(builder);
        //AddSignalR(builder);
        //AddLocalization(builder);
        //AddMvc(builder);
        //AddJsonOptions(builder);
        //AddOpenApi(builder);
        //AddAuth(builder);
        //AddDistributedLock(builder);
        AddDbContext(builder);
        //AddScheduler(builder);
        //AddProfile(builder);
    }

    public virtual void Configure(WebApplication app)
    {
    }
}

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
    }
}
