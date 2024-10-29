using Wta.Infrastructure.Locking;

namespace Wta.Infrastructure.Modules;

public static class Extensions
{
    public static WebApplicationBuilder AddApplication<TApplication>(this WebApplicationBuilder builder)
      where TApplication : class, IApplication
    {
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
                    var strategy = dbContext.Database.CreateExecutionStrategy();
                    strategy.Execute(() =>
                    {
                        using var transaction = dbContext.Database.BeginTransaction();
                        dbContext.Database.Migrate();
                        transaction.Commit();
                    });
                }
            }
        }
        return app;
    }
}
