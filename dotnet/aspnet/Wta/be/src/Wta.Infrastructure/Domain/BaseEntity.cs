namespace Wta.Infrastructure.Domain;

public abstract class BaseEntity : IResource
{
    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    public bool IsDeleted { get; set; }
    public Guid? TenantId { get; set; }
}
