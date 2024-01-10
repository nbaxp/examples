using Wta.Infrastructure.Domain;

namespace Wta.Domain.Identity;

public class RolePermission : Entity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }

}
