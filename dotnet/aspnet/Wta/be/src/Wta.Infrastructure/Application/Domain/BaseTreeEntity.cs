namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseTreeEntity<T> : BaseEntity, IOrderedEntity where T : BaseEntity
{
    public string Name { get; set; } = default!;

    [ReadOnly(true)]
    [RegularExpression(@"\w+")]
    public string Number { get; set; } = default!;

    public int Order { get; set; }

    [Hidden]
    public string Path { get; set; } = default!;

    public Guid? ParentId { get; set; }

    [Hidden]
    public T? Parent { get; set; }
    [Hidden]
    public List<T> Children { get; set; } = [];
}
