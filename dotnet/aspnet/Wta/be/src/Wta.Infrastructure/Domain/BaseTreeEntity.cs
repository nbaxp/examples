using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wta.Infrastructure.Domain;

public abstract class BaseTreeEntity<T> : BaseEntity where T : BaseEntity
{
    public List<T> Children { get; set; } = new List<T>();

    [Required]
    public string Name { get; set; } = null!;

    [ReadOnly(true)]
    public string Number { get; set; } = null!;

    public T? Parent { get; set; }

    public Guid? ParentId { get; set; }

    public string? InternalPath { get; set; }
}
