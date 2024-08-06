namespace Wta.Infrastructure.Application.Domain;

public abstract class Entity : BaseEntity, IConcurrencyStampEntity
{
    public Entity()
    {
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    [Hidden]
    public string ConcurrencyStamp { get; set; }
}
