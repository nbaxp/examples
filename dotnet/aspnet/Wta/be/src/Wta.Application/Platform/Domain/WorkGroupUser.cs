using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class WorkGroupUser : ITenant, IEntityTypeConfiguration<WorkGroupUser>
{
    public Guid WorkGroupId { get; set; }
    public Guid UserId { get; set; }
    public string? TenantNumber { get; set; }
    public WorkGroup? WorkGroup { get; set; }
    public User? User { get; set; }

    public void Configure(EntityTypeBuilder<WorkGroupUser> builder)
    {
        builder.HasKey(o => new { o.WorkGroupId, o.UserId });
        builder.HasOne(o => o.WorkGroup).WithMany(o => o.WorkGroupUsers).HasForeignKey(o => o.WorkGroupId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.User).WithMany(o => o.WorkGroupUsers).HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
