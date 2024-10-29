namespace Wta.Infrastructure.Modules;

public interface IApplication
{
    void Configure(WebApplication app);

    void ConfigureServices(WebApplicationBuilder builder);
}
