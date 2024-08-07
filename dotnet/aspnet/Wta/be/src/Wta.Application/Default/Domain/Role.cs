using System.ComponentModel.DataAnnotations.Schema;

namespace Wta.Application.Default.Domain;

[SystemManagement, Display(Name = "角色", Order = 4)]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;

    [Hidden]
    public List<UserRole> UserRoles { get; set; } = [];
    [Hidden]
    public List<RolePermission> RolePermissions { get; set; } = [];

    [KeyValue("hideForList",true)]
    [UIHint("select")]
    [KeyValue("url", "permission/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [NotMapped]
    public List<Guid> Permissions
    {
        get { return RolePermissions.Select(o => o.PermissionId).ToList(); }
    }
}
