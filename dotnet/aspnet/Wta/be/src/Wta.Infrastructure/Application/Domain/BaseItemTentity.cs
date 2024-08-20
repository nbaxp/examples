namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseChildTentity<T> : BaseEntity, IOrderedEntity where T : BaseEntity
{
    public virtual Guid ParentId { get; set; }

    [Hidden]
    public T? Parent { get; set; }

    [DefaultValue(0)]
    public float Order { get; set; }
}
