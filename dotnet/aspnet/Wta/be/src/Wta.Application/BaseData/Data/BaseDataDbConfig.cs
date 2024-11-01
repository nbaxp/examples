using Wta.Application.BaseData.Domain;
using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Data;

public class BaseDataDbConfig : BaseDbConfig<PlatformDbContext>,
    IEntityTypeConfiguration<Workstation>,
    IEntityTypeConfiguration<Asset>,
    IEntityTypeConfiguration<WorkstationDevice>,
    IEntityTypeConfiguration<WorkstationTimeGroup>,
    IEntityTypeConfiguration<TimeRange>
{
    public void Configure(EntityTypeBuilder<Workstation> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Workstations).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Assets).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<WorkstationDevice> builder)
    {
        builder.HasKey(o => new { o.WorkstationId, o.AssetId });
        builder.HasOne(o => o.Workstation).WithMany(o => o.WorkstationDevices).HasForeignKey(o => o.WorkstationId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Asset).WithMany(o => o.WorkstationDevices).HasForeignKey(o => o.AssetId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<WorkstationTimeGroup> builder)
    {
        builder.HasOne(o => o.Workstation).WithMany(o => o.TimeGroups).HasForeignKey(o => o.WorkstationId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<TimeRange> builder)
    {
        builder.HasOne(o => o.Group).WithMany(o => o.Ranges).HasForeignKey(o => o.GroupId).OnDelete(DeleteBehavior.Cascade);
    }
}
