using Wta.Application.Default.Attributes;
using Wta.Infrastructure.Domain;

namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Order = 3)]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;
    public List<UserRole> UserRoles { get; set; } = [];
    public List<RolePermission> RolePermissions { get; set; } = [];
}
