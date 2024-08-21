namespace Wta.Infrastructure.Application.Domain;

public abstract class BaseNameNumberEntity : BaseEntity
{
    [DisplayOrder(-2)]
    public string Name { get; set; } = default!;

    [ReadOnly(true)]
    [RegularExpression(@"\w+")]
    [DisplayOrder(-1)]
    public string Number { get; set; } = default!;
}
