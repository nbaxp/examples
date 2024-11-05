using Wta.Application.BaseData.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Tpm.Domain;

[Display(Name = "点检计划",Order =30)]
[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class CheckPlan : BaseNameNumberEntity
{
    public List<PlanProject> PlanProjects { get; set; } = [];
}

[Display(Name = "点检项目",Order =20)]
[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class CheckProject : BaseNameNumberEntity
{
    public List<PlanProject> PlanProjects { get; set; } = [];
    public List<ProjectStandard> PorjectStandards { get; set; } = [];
}

[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class PlanProject : ISoftDelete, ITenant, IEntityTypeConfiguration<PlanProject>
{
    public Guid PlanId { get; set; }
    public Guid ProjectId { get; set; }
    public CheckPlan? Plan { get; set; }
    public CheckProject? Project { get; set; }
    public bool IsDeleted { get; set; }

    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<PlanProject> builder)
    {
        builder.HasKey(o => new { o.PlanId, o.ProjectId });
        builder.HasOne(o => o.Plan).WithMany(o => o.PlanProjects).HasForeignKey(o => o.PlanId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Project).WithMany(o => o.PlanProjects).HasForeignKey(o => o.ProjectId).OnDelete(DeleteBehavior.Cascade);
    }
}

[Display(Name = "点检标准",Order =10)]
[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class CheckStandard : BaseNameNumberEntity, IEntityTypeConfiguration<CheckStandard>
{
    public List<ProjectStandard> PorjectStandards { get; set; } = [];
    public List<AttributeMeta> Attributes { get; set; } = [];

    public void Configure(EntityTypeBuilder<CheckStandard> builder)
    {
        builder.OwnsMany(o => o.Attributes, o => o.ToJson());
    }
}

[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class ProjectStandard : ISoftDelete, ITenant, IEntityTypeConfiguration<ProjectStandard>
{
    public Guid ProjectId { get; set; }
    public Guid StandardId { get; set; }
    public CheckProject? Project { get; set; }
    public CheckStandard? Standard { get; set; }
    public bool IsDeleted { get; set; }

    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<ProjectStandard> builder)
    {
        builder.HasKey(o => new { o.ProjectId, o.StandardId });
        builder.HasOne(o => o.Project).WithMany(o => o.PorjectStandards).HasForeignKey(o => o.ProjectId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Standard).WithMany(o => o.PorjectStandards).HasForeignKey(o => o.StandardId).OnDelete(DeleteBehavior.Cascade);
    }
}
