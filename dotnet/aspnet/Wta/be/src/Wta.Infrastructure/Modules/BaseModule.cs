namespace Wta.Infrastructure.Modules;

public abstract class BaseModule : IModule
{
    public virtual void Configure(WebApplication app)
    {
    }

    public virtual void ConfigureServices(WebApplicationBuilder builder)
    {
    }
}
