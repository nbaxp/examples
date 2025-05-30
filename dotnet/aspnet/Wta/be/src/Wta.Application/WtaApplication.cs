using Wta.Application.Platform;
using Wta.Infrastructure.Modules;

namespace Wta.Application;

public class WtaApplication : BaseApplication
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        base.ConfigureServices(builder);
        builder.AddModule<PlatformModule>();
    }
}
