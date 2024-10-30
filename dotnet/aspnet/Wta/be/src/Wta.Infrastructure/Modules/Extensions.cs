using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Wta.Infrastructure.Locking;

namespace Wta.Infrastructure.Modules;

public static class Extensions
{
    public static WebApplicationBuilder AddApplication<TApplication>(this WebApplicationBuilder builder)
      where TApplication : class, IApplication
    {
        if (builder.Configuration.GetValue("DOTNET_RUNNING_IN_CONTAINER", false))
        {
            builder.Configuration
                .AddJsonFile("appsettings.Container.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Container.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        }
        var application = Activator.CreateInstance<TApplication>();
        application.ConfigureServices(builder);
        builder.Services.AddSingleton(application);
        builder.Services.AddSingleton<IApplication>(application);
        return builder;
    }

    public static WebApplicationBuilder AddModule<TModule>(this WebApplicationBuilder builder)
      where TModule : class, IModule
    {
        var module = Activator.CreateInstance<TModule>();
        module.ConfigureServices(builder);
        builder.Services.AddSingleton(module);
        builder.Services.AddSingleton<IModule>(module);
        return builder;
    }

    public static WebApplication UseModules(this WebApplication app)
    {
        //Global.Application = app; // delete
        app.Services.GetRequiredService<IApplication>().Configure(app);
        foreach (var module in app.Services.GetServices<IModule>())
        {
            module.Configure(app);
        }
        if (app.Environment.IsDevelopment())
        {
            using var scope = app.Services.CreateScope();
            foreach (var dbContext in scope.ServiceProvider.GetServices<DbContext>())
            {
                var @lock = app.Services.GetRequiredService<ILock>();
                using var handle = @lock.Acquire("seed");
                if (handle != null)
                {
                    if (dbContext.Database.EnsureCreated())
                    {
                        using var transaction = dbContext.Database.BeginTransaction();
                        var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContext.GetType());
                        var list = scope.ServiceProvider.GetServices(dbSeedType)
                        .OrderBy(o => o!.GetType().GetAttribute<DisplayAttribute>()?.GetOrder() ?? 0)
                        .ToList();
                        foreach (var item in list)
                        {
                            dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(item, [dbContext]);
                        }
                        transaction.Commit();
                    }
                }
            }
        }
        else
        {
            using var scope = app.Services.CreateScope();
            foreach (var dbContext in scope.ServiceProvider.GetServices<DbContext>())
            {
                var @lock = app.Services.GetRequiredService<ILock>();
                using var handle = @lock.Acquire("seed");
                if (handle != null)
                {
                    var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>() as RelationalDatabaseCreator;
                    var dbExist = dbCreator!.Exists();
                    if (!dbExist)
                    {
                        dbCreator.Create();
                    }
                    var strategy = dbContext.Database.CreateExecutionStrategy();
                    strategy.Execute(() =>
                    {
                        using var transaction = dbContext.Database.BeginTransaction();
                        dbContext.Database.Migrate();
                        if (!dbExist)
                        {
                            var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContext.GetType());
                            var list = scope.ServiceProvider.GetServices(dbSeedType)
                            .OrderBy(o => o!.GetType().GetAttribute<DisplayAttribute>()?.GetOrder() ?? 0)
                            .ToList();
                            foreach (var item in list)
                            {
                                dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(item, [dbContext]);
                            }
                        }
                        transaction.Commit();
                    });
                }
            }
        }
        return app;
    }
}
