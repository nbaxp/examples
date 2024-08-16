namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseEntity : IResource
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    [Hidden]
    public Guid Id { get; set; }

    [DisplayOrder(10000)]
    [Hidden]
    public DateTime CreatedOn { get; set; }

    [DisplayOrder(10001)]
    [Hidden]
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
    public string? TenantNumber { get; set; }
}
