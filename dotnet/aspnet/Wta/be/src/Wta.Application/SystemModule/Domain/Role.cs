using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[PermissionManagement, Display(Name = "角色", Order = 4)]
[DependsOn<SystemDbContext>]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;

    [Hidden]
    public List<UserRole> UserRoles { get; set; } = [];

    [Hidden]
    public List<RolePermission> RolePermissions { get; set; } = [];

    [KeyValue("hideForList", true)]
    [UIHint("select")]
    [KeyValue("url", "permission/tenant")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [NotMapped]
    public List<Guid> Permissions { get; set; } = [];
}
