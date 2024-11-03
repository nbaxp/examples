using Wta.Application.BaseData.Domain;
using Wta.Application.Oee;
using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Data;

public class BaseDataDbConfig : BaseDbConfig<PlatformDbContext>,
    IEntityTypeConfiguration<Uom>,
    IEntityTypeConfiguration<Workstation>,
    IEntityTypeConfiguration<OeeConfiguration>,
    IEntityTypeConfiguration<Asset>,
    IEntityTypeConfiguration<AssetVersion>,
    IEntityTypeConfiguration<WorkstationDevice>,
    IEntityTypeConfiguration<WorkstationTimeGroup>,
    IEntityTypeConfiguration<TimeRange>
{
    public void Configure(EntityTypeBuilder<Uom> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Uoms).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Workstation> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Workstations).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Assets).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
        //builder.ToJsonExt(o => o.Properties, o => o.Properties);
        builder.OwnsMany(o => o.Values, o => o.ToJson());
        //builder.Property(o => o.Values).HasColumnType("json");
    }

    public void Configure(EntityTypeBuilder<AssetVersion> builder)
    {
        builder.OwnsMany(o => o.Attributes, o => o.ToJson());
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

    public void Configure(EntityTypeBuilder<OeeConfiguration> builder)
    {
        //builder.Property(o => o.Numerator).HasConversion(
        //v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        //v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null),
        //new ValueComparer<ICollection<string>>(
        //    (c1, c2) => c1.SequenceEqual(c2),
        //    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        //    c => (ICollection<string>)c.ToList()));
        //builder.Property(o => o.Denominator).HasConversion(
        //v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
        //v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null),
        //new ValueComparer<ICollection<string>>(
        //    (c1, c2) => c1.SequenceEqual(c2),
        //    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        //    c => (ICollection<string>)c.ToList()));

        //builder.PrimitiveCollection(o => o.Numerator);
        //builder.PrimitiveCollection(o => o.Denominator);
        //builder.OwnsMany(o => o.Numerator, o => o.ToJson());
        //builder.OwnsMany(o => o.Denominator, o => o.ToJson());
    }
}
