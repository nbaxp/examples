using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[IdentityGroup, Display(Order = 2)]
public class Role : Entity
{
    public string? Name { get; set; }
    public string? Number { get; set; }
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
