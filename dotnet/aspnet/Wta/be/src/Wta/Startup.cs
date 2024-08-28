using Wta.Application.BaseModule;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Startup;

namespace Wta.Application;

[DependsOn<DefaultModule>]
public class Startup : BaseStartup
{
}
