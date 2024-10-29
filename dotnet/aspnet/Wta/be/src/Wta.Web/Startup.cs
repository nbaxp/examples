using Wta.Application.Platform;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Startup;

namespace Wta.Web;

[DependsOn<PlatformModule>]
public class Startup : BaseStartup
{
}
