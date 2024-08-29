using Wta.Application.BaseModule;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Startup;

namespace Wta.Application;

[DependsOn<BaseModule.Module>]
public class Startup : BaseStartup
{
}
