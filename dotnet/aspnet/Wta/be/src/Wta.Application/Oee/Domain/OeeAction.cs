using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE事件", Order = 70)]
public class OeeAction : BaseEntity
{
    [Display(Name = "标题")]
    public string Title { get; set; } = default!;

    [Display(Name = "TEAMID")]
    public string? TeamId { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "userName")]
    [KeyValue("label", "name")]
    [Display(Name = "创建人")]
    public string? Owner { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "userName")]
    [KeyValue("label", "name")]
    [Display(Name = "分配")]
    public string? Assignee { get; set; } = default!;

    [Display(Name = "提出日期")]
    public DateOnly DateRaised { get; set; }

    [Display(Name = "到期日期")]
    public DateOnly DateDue { get; set; }

    [UIHint("select")]
    [KeyValue("url", "oee-action-category/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [Display(Name = "分类")]
    public string CategoryNumber { get; set; } = default!;

    [Display(Name = "优先级")]
    public OeePriority Priority { get; set; }

    [Hidden]
    public List<OeeActionRequirement> ActionRequirements { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "oee-requirement/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [NotMapped]
    [Display(Name = "处理建议")]
    public List<Guid> Requirements
    {
        get
        {
            return this.ActionRequirements?.Select(o => o.RequirementId).ToList() ?? [];
        }
        set
        {
            this.ActionRequirements = value?.Distinct().Select(o => new OeeActionRequirement { RequirementId = o }).ToList() ?? [];
        }
    }

    [DataType(DataType.MultilineText)]
    [Display(Name = "详情")]
    public string Details { get; set; } = default!;

    [Display(Name = "PLANT")]
    public string Plant { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "oee-shift/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public string? ShiftNumber { get; set; } = default!;

    [Display(Name = "PART")]
    public string Part { get; set; } = default!;

    [Display(Name = "附件")]
    public List<string> Attachments { get; set; } = [];
}
