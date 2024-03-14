namespace Wta.Infrastructure.Interfaces;

public interface ITenantService
{
    string? TenantNumber { get; set; }
    List<string> Permissions { get; set; }
}
