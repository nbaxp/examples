namespace Wta.Infrastructure.Interfaces;

public interface ITenantService
{
    Guid? TenantId { get; set; }
    List<string> Permissions { get; set; }
}
