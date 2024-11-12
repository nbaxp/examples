using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "事件原因", Order = 40)]
public class OeeReason : BaseTreeEntity<OeeReason>
{
    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "#ffffff";

    public List<OeeRange> Ranges { get; set; } = [];
}
