using Microsoft.AspNetCore.Builder;

namespace Wta.Infrastructure;

public abstract class BaseModule : IModule
{
  public virtual void Configure(WebApplication app)
  {
  }

  public virtual void ConfigureServices(WebApplicationBuilder builder)
  {
  }
}
