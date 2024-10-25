using Microsoft.AspNetCore.Builder;
namespace Wta.Infrastructure.Module;

public abstract class BaseApplicationModule : IApplicationModule
{
    public virtual void ConfigureModule(WebApplication app)
    {
    }

    public virtual void ConfigureModuleServices(WebApplicationBuilder builder)
    {
    }

    //public virtual void Seed(TDbContext context)
    //{
    //}
}