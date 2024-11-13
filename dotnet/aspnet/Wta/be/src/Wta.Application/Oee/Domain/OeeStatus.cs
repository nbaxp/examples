using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE状态", Order = 30)]
public class OeeStatus : BaseTreeEntity<OeeStatus>
{
    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = "#ffffff";

    public List<OeeData> Datas { get; set; } = [];
}
