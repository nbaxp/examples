using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class UserRole : ISoftDelete, ITenant
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }
}
