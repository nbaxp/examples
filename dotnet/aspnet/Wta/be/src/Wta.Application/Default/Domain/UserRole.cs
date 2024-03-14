using Wta.Infrastructure.Domain;

namespace Wta.Application.Default.Domain;

public class UserRole : ITenantEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
    public bool IsReadOnly { get; set; }
    public string? TenantNumber { get; set; }
}
