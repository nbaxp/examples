namespace Wta.Infrastructure.Domain;

public abstract class Entity : BaseEntity, IOrderedEntity, IConcurrencyStampEntity, ISoftDeletedEntity
{
    public Entity()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public int Order { get; set; }
    public string ConcurrencyStamp { get; set; }
    public bool IsDeleted { get; set; }
    public Guid TenantId { get; set; }
}
