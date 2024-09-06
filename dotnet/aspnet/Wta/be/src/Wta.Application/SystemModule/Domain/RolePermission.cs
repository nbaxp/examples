using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[DependsOn<SystemDbContext>]
public class RolePermission : ITenantEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
    public bool IsReadOnly { get; set; }
    public string? TenantNumber { get; set; }
}
