using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class RolePermission : ISoftDelete, ITenant
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }
}
