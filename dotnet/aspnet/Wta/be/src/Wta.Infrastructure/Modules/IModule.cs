namespace Wta.Infrastructure.Modules;

public interface IModule
{
    void Configure(WebApplication app);

    void ConfigureServices(WebApplicationBuilder builder);
}
