using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE处理建议", Order = 60)]
public class OeeRequirement : BaseNameNumberEntity
{
    [Hidden]
    public List<OeeActionRequirement> ActionRequirements { get; set; } = [];
}
