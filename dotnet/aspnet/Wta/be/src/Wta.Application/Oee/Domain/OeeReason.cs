using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE原因", Order = 30)]
public class OeeReason : BaseNameNumberEntity
{
    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "#CC0000";

    [Display(Name = "启用")]
    public bool Enabled { get; set; } = true;
}
