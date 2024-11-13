using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE事件状态", Order = 60)]
public class OeeActionStatus : BaseNameNumberEntity
{
}
