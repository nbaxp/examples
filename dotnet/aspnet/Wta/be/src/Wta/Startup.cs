using Wta.Application.SystemModule;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Startup;

namespace Wta.Application;

[DependsOn<Module>]
public class Startup : BaseStartup
{
}
