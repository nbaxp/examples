using Wta.Application.SystemModule.Data;
using Wta.Infrastructure.Modules;

namespace Wta.Application;
public class WtaApplication : BaseApplication
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        base.ConfigureServices(builder);
        builder.AddDbContext<SystemDbContext>();
    }
    public override void Configure(WebApplication app)
    {
    }
}
