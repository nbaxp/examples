namespace Wta.Infrastructure.Domain;

public interface ISoftDeletedEntity
{
    bool IsDeleted { get; set; }
}
