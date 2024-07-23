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
    public DateTime CreatedOn { get; set; }

    [DisplayOrder(10001)]
    public string CreatedBy { get; set; } = default!;

    [DisplayOrder(10002)]
    public DateTime? UpdatedOn { get; set; }

    [DisplayOrder(10003)]
    public string? UpdatedBy { get; set; }

    [DisplayOrder(10004)]
    public bool IsDeleted { get; set; }

    [Hidden]
    public string? TenantNumber { get; set; }
}
