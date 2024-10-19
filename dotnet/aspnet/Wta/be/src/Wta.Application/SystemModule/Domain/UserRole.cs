using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[DependsOn<SystemDbContext>]
public class UserRole : ISoftDelete, ITenantEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }
}
