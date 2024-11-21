namespace Wta.Application.Oee.Resources;

[Oee]
[Display(Name = "OEE分析", Order = 1)]
public class OeeAnalysis : IResource
{
    [Display(Name = "日期")]
    public DateOnly Day { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [UIHint("select")]
    [KeyValue("url", "oee-asset/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "资产")]
    public string AssetNumber { get; set; } = default!;
}
