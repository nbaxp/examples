using Wta.Application.Platform.Data;
using Wta.Infrastructure.Modules;

namespace Wta.Application.Platform;

public class PlatformModule : BaseModule
{
    public override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.AddDbContext<PlatformDbContext>();
    }
}
