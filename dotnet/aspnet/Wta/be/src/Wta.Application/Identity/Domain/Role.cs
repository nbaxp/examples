using Wta.Application.Identity.Attributes;
using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[SystemManagement, Display(Order = 2)]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
