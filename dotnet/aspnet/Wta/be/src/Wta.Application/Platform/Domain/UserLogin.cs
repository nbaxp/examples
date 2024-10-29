using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[PermissionManagement, Display(Name = "登录绑定", Order = 6)]
[DependsOn<PlatformDbContext>]
public class UserLogin : BaseEntity
{
    public string LoginProvider { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
