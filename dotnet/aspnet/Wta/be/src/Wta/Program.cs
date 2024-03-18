using Wta.Application.Default;
using Wta.Infrastructure;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Startup;

WtaApplication.Run<Startup>(args);

[DependsOn<DefaultModule>]
public class Startup : BaseStartup
{
}
