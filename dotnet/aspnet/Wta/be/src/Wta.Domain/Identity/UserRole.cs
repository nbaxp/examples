using Wta.Infrastructure.Domain;

namespace Wta.Domain.Identity;

public class UserRole : Entity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
}
