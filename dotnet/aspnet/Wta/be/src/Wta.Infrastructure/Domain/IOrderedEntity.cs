namespace Wta.Infrastructure.Domain;

public interface IOrderedEntity
{
    int Order { get; set; }
}
