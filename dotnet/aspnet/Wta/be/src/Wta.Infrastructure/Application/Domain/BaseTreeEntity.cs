namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseTreeEntity<T> : BaseNameNumberEntity, IOrdered where T : BaseEntity
{
    [DefaultValue(0)]
    public float Order { get; set; }

    [Hidden]
    public string Path { get; set; } = default!;

    [KeyValue("hideForList", true)]
    public virtual Guid? ParentId { get; set; }

    [Hidden]
    public T? Parent { get; set; }

    [Hidden]
    public List<T> Children { get; set; } = [];
}
