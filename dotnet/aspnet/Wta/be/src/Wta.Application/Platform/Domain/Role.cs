using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[PermissionManagement, Display(Name = "角色", Order = 4)]
[DependsOn<PlatformDbContext>]
public class Role : Entity
{
    public string Name { get; set; } = default!;
    public string Number { get; set; } = default!;

    [Hidden]
    [JsonIgnore]
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
    public List<Guid> Permissions
    {
        get
        {
            return this.RolePermissions?.Select(o => o.PermissionId).ToList() ?? [];
        }
        set
        {
            this.RolePermissions = value?.Distinct().Select(o => new RolePermission { PermissionId = o }).ToList() ?? [];
        }
    }
}
