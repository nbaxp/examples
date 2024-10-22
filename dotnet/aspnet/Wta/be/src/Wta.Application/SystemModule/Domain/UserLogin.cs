using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[PermissionManagement, Display(Name = "登录绑定", Order = 6)]
[DependsOn<SystemDbContext>]
public class UserLogin : BaseEntity
{
    public string LoginProvider { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
