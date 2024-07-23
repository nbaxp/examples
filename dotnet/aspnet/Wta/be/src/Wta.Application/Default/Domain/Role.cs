namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Name = "角色", Order = 3)]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;

    [Hidden]
    public List<UserRole> UserRoles { get; set; } = [];
    [Hidden]
    public List<RolePermission> RolePermissions { get; set; } = [];
}
