using Microsoft.AspNetCore.Builder;

namespace Wta.Infrastructure.Hosting;

public interface IStartup
{
    void Configure(WebApplication webApplication);

    void ConfigureServices(WebApplicationBuilder builder);

    void Initialize();
}
