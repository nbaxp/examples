namespace Wta.Infrastructure.Domain;

public interface ITenantEntity
{
    public Guid? TenantId { get; set; }
}
