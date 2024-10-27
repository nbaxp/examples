using Microsoft.AspNetCore.Builder;

namespace Wta.Infrastructure;

public interface IModule
{
  void Configure(WebApplication app);

  void ConfigureServices(WebApplicationBuilder builder);
}
