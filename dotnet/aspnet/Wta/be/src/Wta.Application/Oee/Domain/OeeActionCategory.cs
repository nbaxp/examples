using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE事件分类", Order = 50)]
public class OeeActionCategory : BaseNameNumberEntity
{
}
