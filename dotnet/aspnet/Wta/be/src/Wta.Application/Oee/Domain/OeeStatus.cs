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

    [Display(Name = "类型")]
    public OeeStatusType Type { get; set; }

    public List<OeeData> Datas { get; set; } = [];
}

public enum OeeStatusType
{
    [Display(Name = "正常生产")]
    ProductionTime = 10,

    [Display(Name = "无负荷时间")]
    UnloadedTime = 20,

    [Display(Name = "计划停机")]
    PlannedDowntime = 30,

    [Display(Name = "非计划停机")]
    UnplannedDowntime = 40,
}
