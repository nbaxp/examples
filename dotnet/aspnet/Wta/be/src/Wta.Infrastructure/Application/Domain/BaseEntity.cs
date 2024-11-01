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
    public DateTime CreatedOn { get; set; }

    [DisplayOrder(10001)]
    [Hidden]
    [ReadOnly(true)]
    public string CreatedBy { get; set; } = default!;

    [DisplayOrder(10002)]
    [Hidden]
    public DateTime? UpdatedOn { get; set; }

    [DisplayOrder(10003)]
    [Hidden]
    public string? UpdatedBy { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayOrder(10004)]
    public string? Remark { get; set; }

    [DisplayOrder(10005)]
    [KeyValue("hideForEdit", true)]
    [KeyValue("hideForList", true)]
    public bool IsDeleted { get; set; }

    [Hidden]
    [ReadOnly(true)]
    public string? TenantNumber { get; set; }
}
