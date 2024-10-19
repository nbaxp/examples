namespace Wta.Infrastructure.Application.Domain;
public interface ISoftDelete
{
    bool IsDeleted { get; }
}
