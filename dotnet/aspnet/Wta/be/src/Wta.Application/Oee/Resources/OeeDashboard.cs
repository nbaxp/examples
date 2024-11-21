using Wta.Infrastructure;

namespace Wta.Application.Oee.Resources;

[Oee]
[Display(Name = "OEE仪表盘", Order = -10)]
public class OeeDashboard : IResource, IValidatableObject
{
    [Display(Name = "开始日期")]
    public DateOnly Start { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

    [Display(Name = "结束日期")]
    public DateOnly End { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [UIHint("select")]
    [KeyValue("url", "oee-asset/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [KeyValue("checkStrictly", true)]
    [Display(Name = "资产")]
    public string? AssetNumber { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-shift/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public string? ShiftNumber { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        using var scope = Global.Application.Services.CreateScope();
        var stringLocalizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer>();
        if (this.Start >= this.End)
        {
            yield return new ValidationResult(stringLocalizer["开始日期小于结束时间"], [nameof(Start)]);
        }
    }
}
