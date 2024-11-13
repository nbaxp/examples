namespace Wta.Application.Oee.Resources;

[Oee]
[Display(Name = "OEE仪表盘", Order = 0)]
public class OeeDashboard : IResource
{
    [Display(Name = "开始日期")]
    public DateOnly Start { get; set; }

    [Display(Name = "结束日期")]
    public DateOnly End { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-asset/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "资产")]
    public string? AssetNumber { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-shift/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public string? ShiftNumber { get; set; }
}
