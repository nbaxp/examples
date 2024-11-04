using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class UserRole : ISoftDelete, ITenant, IEntityTypeConfiguration<UserRole>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    public User? User { get; set; }
    public Role? Role { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(o => new { o.UserId, o.RoleId });
        builder.HasOne(o => o.User).WithMany(o => o.UserRoles).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Role).WithMany(o => o.UserRoles).HasForeignKey(o => o.RoleId).OnDelete(DeleteBehavior.Cascade);
    }
}
