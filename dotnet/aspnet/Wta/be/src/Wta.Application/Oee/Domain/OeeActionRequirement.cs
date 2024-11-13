using Wta.Application.Platform;

namespace Wta.Application.Oee.Domain;

[DependsOn<PlatformDbContext>]
public class OeeActionRequirement : ISoftDelete, ITenant, IEntityTypeConfiguration<OeeActionRequirement>
{
    public Guid ActionId { get; set; }
    public Guid RequirementId { get; set; }
    public OeeAction? Action { get; set; }
    public OeeRequirement? Requirement { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<OeeActionRequirement> builder)
    {
        builder.HasKey(o => new { o.ActionId, o.RequirementId });
        builder.HasOne(o => o.Action).WithMany(o => o.ActionRequirements).HasForeignKey(o => o.ActionId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Requirement).WithMany(o => o.ActionRequirements).HasForeignKey(o => o.RequirementId).OnDelete(DeleteBehavior.Cascade);
    }
}
