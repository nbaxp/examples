using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class RolePermission : ISoftDelete, ITenant,IEntityTypeConfiguration<RolePermission>
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(o => new { o.RoleId, o.PermissionId });
        builder.HasOne(o => o.Role).WithMany(o => o.RolePermissions).HasForeignKey(o => o.RoleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Permission).WithMany(o => o.RolePermissions).HasForeignKey(o => o.PermissionId).OnDelete(DeleteBehavior.Cascade);
    }
}
