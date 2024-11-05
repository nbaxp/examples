namespace Wta.Application.Platform.Domain;

[PermissionManagement, Display(Name = "登录绑定", Order = 6)]
[DependsOn<PlatformDbContext>]
public class UserLogin : BaseEntity, IEntityTypeConfiguration<UserLogin>
{
    public string LoginProvider { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string ProviderKey { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.HasOne(o => o.User).WithMany(o => o.UserLogins).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
