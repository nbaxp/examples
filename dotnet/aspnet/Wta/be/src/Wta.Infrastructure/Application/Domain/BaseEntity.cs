namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseEntity : IResource, ISoftDelete, ITenant
{
    public BaseEntity()
    {
        var dbContextType = this.GetType().GetCustomAttribute(typeof(DependsOnAttribute<>))!.GetType().GenericTypeArguments.First();
        Id = ((DbContext)Global.Application.Services.GetRequiredService(dbContextType)).NewGuid();
    }

    [Hidden]
    [ReadOnly(true)]
    public Guid Id { get; set; }

    [DisplayOrder(10000)]
    [Hidden]
    [ReadOnly(true)]
    [Display(Name = "创建时间")]
    public DateTime CreatedOn { get; set; }

    [DisplayOrder(10001)]
    [Hidden]
    [ReadOnly(true)]
    [Display(Name = "创建人")]
    public string CreatedBy { get; set; } = default!;

    [DisplayOrder(10002)]
    [Hidden]
    [Display(Name = "更新时间")]
    public DateTime? UpdatedOn { get; set; }

    [DisplayOrder(10003)]
    [Hidden]
    [Display(Name = "更新人")]
    public string? UpdatedBy { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayOrder(10004)]
    [Display(Name = "备注")]
    public string? Remark { get; set; }

    [DisplayOrder(10005)]
    [KeyValue("hideForEdit", true)]
    [KeyValue("hideForList", true)]
    [Display(Name = "已归档")]
    public bool IsDeleted { get; set; }

    [Hidden]
    [ReadOnly(true)]
    [Display(Name = "租户编号")]
    public string? TenantNumber { get; set; }
}
