using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[Oee]
[DependsOn<PlatformDbContext>]
[Display(Name = "OEE事件", Order = 70)]
public class OeeAction : BaseEntity, IEntityTypeConfiguration<OeeAction>
{
    [Display(Name = "标题")]
    public string Title { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "oee-action-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "分类")]
    public Guid CategoryId { get; set; } = default!;

    //[Display(Name = "TEAMID")]
    //public string? TeamId { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "发送人")]
    public Guid SenderId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "user/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "接收人")]
    public Guid ReceiverId { get; set; }

    [Display(Name = "提出日期")]
    public DateOnly DateRaised { get; set; }

    [Display(Name = "到期日期")]
    public DateOnly DateDue { get; set; }

    [Display(Name = "优先级")]
    public OeePriority Priority { get; set; }

    [Display(Name = "已读")]
    public bool IsRead { get; set; } = default!;

    [DataType(DataType.MultilineText)]
    [Display(Name = "详情")]
    public string Details { get; set; } = default!;

    //[Display(Name = "PLANT")]
    //public string Plant { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "oee-shift/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public string? ShiftNumber { get; set; } = default!;

    [Display(Name = "PART")]
    public string Part { get; set; } = default!;
    public OeeActionCategory? Category { get; set; }

    public User? Sender { get; set; } = default!;
    public User? Receiver { get; set; } = default!;

    [Display(Name = "附件")]
    public List<string> Attachments { get; set; } = [];

    public void Configure(EntityTypeBuilder<OeeAction> builder)
    {
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Sender).WithMany().HasForeignKey(o => o.SenderId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Receiver).WithMany().HasForeignKey(o => o.ReceiverId).OnDelete(DeleteBehavior.Cascade);
    }
}
