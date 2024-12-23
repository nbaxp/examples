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
        AddLocalEventBus(builder);
        AddHttpServices(builder);
        AddContentProvider(builder);
        AddCors(builder);
        AddCache(builder);
        AddLock(builder);
        AddRouting(builder);
        AddSignalR(builder);
        AddFileProvider(builder);
        AddLocalization(builder);
        AddMvc(builder);
        AddJsonOptions(builder);
        AddOpenApi(builder);
        AddAuth(builder);
        AddDistributedLock(builder);
        AddScheduler(builder);
        AddDbContext(builder);
        AddRepository(builder);
        AddMqttServer(builder);
    }

    public virtual void Configure(WebApplication app)
    {
        Global.Application = app;
        UseMonitoring(app);
        UseLogging(app);
        UseStaticFiles(app);
        UseRouting(app);
        UseOpenApi(app);
        UseAuth(app);
        UseEndpoints(app);
        UseSignalR(app);
        UseMqttServer(app);
        UseCORS(app);
        UseLocalization(app);
        UseDbContext(app);
        UseScheduler(app);
    }
}
