using Wta.Infrastructure.Domain;

namespace Wta.Domain.Identity;

public class Role : Entity
{
    public string? Name { get; set; }
    public string? Number { get; set; }
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
