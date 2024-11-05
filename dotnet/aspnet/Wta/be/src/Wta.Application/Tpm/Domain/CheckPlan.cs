using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.BaseData.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Tpm.Domain;

[Display(Name = "点检计划", Order = 30)]
[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class CheckPlan : BaseNameNumberEntity
{
    public List<PlanProject> PlanProjects { get; set; } = [];

    [KeyValue("hideForList", true)]
    [UIHint("select")]
    [KeyValue("url", "check-project/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [NotMapped]
    [Display(Name = "项目")]
    public List<Guid> Projects
    {
        get
        {
            return this.PlanProjects?.Select(o => o.ProjectId).ToList() ?? [];
        }
        set
        {
            this.PlanProjects = value?.Distinct().Select(o => new PlanProject { ProjectId = o }).ToList() ?? [];
        }
    }
}

[Display(Name = "点检项目", Order = 20)]
[DependsOn<PlatformDbContext>]
[DevicePlanCheck]
public class CheckProject : BaseNameNumberEntity
{
    public List<PlanProject> PlanProjects { get; set; } = [];
    public List<ProjectStandard> PorjectStandards { get; set; } = [];

    [KeyValue("hideForList", true)]
    [UIHint("select")]
    [KeyValue("url", "check-standard/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [NotMapped]
    [Display(Name = "标准")]
    public List<Guid> Standards
    {
        get
        {
            return this.PorjectStandards?.Select(o => o.StandardId).ToList() ?? [];
        }
        set
        {
            this.PorjectStandards = value?.Distinct().Select(o => new ProjectStandard { StandardId = o }).ToList() ?? [];
        }
    }
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

[Display(Name = "点检标准", Order = 10)]
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
