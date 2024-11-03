using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform.Data;

namespace Wta.Application.Oee;

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "设备状态类型", Order = 1)]
public class EqpType : BaseNameNumberEntity
{
}

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "设备状态分类", Order = 2)]
public class EqpCategory : BaseNameNumberEntity
{
    [UIHint("select")]
    [KeyValue("url", "eqp-type/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "状态类型")]
    public string Type { get; set; } = default!;

    [UIHint("color")]
    [Display(Name = "颜色")]
    public string Color { get; set; } = default!;

    [Display(Name = "禁用")]
    public bool Disabled { get; set; }
}

public enum AvailabilitMode
{
    [Display(Name = "班次")]
    SHIFT,

    [Display(Name = "状态")]
    AUTO
}

[OeeBaseData]
[DependsOn<PlatformDbContext>]
[Display(Name = "配置", Order = 3)]
public class OeeConfiguration : BaseEntity//, IValidatableObject
{
    [Display(Name = "配置名称")]
    public string Name { get; set; } = default!;

    [Display(Name = "设为默认")]
    public bool IsDefault { get; set; }

    [UIHint("select")]
    [KeyValue("url", "eqp-category/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [Display(Name = "实际运行时间")]
    public List<string> Numerator { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "eqp-category/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [Display(Name = "计划运行时间")]
    public List<string> Denominator { get; set; } = [];

    [Display(Name = "可用性")]
    [NotMapped]
    public string? Availabilit { get; set; }

    [UIHint("radio")]
    [Display(Name = "可用性模式")]
    public AvailabilitMode AvailabilitMode { get; set; }

    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    if (AvailabilitMode == AvailabilitMode.AUTO)
    //    {
    //        if (Numerator.Count == 0)
    //        {
    //            yield return new ValidationResult("实际运行时间不能为空", [nameof(Numerator).ToLowerCamelCase()]);
    //        }
    //        if (Numerator.Any(o => !Denominator.Any(d => d.Contains(o))))
    //        {
    //            yield return new ValidationResult("实际运行时间必须包含在计划运行时间内", [nameof(Numerator).ToLowerCamelCase()]);
    //        }
    //        if (Denominator.Count == 0)
    //        {
    //            yield return new ValidationResult("计划运行时间不能为空", [nameof(Denominator).ToLowerCamelCase()]);
    //        }
    //    }
    //}
}
