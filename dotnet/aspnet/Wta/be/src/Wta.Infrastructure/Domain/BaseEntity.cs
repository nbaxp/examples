namespace Wta.Infrastructure.Domain;

public abstract class BaseEntity : IResource
{
    public Guid Id { get; set; }

    public BaseEntity()
    {
        this.Id = Guid.NewGuid();
    }
}
