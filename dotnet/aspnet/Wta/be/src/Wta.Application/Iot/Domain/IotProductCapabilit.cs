using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[DependsOn<PlatformDbContext>]
public class IotProductCapabilit : ISoftDelete, ITenant, IEntityTypeConfiguration<IotProductCapabilit>
{
    public Guid ProductId { get; set; }
    public Guid CapabilitId { get; set; }
    public IotProduct? Product { get; set; }
    public IotCapabilit? Capabilit { get; set; }

    public bool IsDeleted { get; set; }

    public string? TenantNumber { get; set; }

    public void Configure(EntityTypeBuilder<IotProductCapabilit> builder)
    {
        builder.HasKey(o => new { o.ProductId, o.CapabilitId });
        builder.HasOne(o => o.Product).WithMany(o => o.ProductCapabilis).HasForeignKey(o => o.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Capabilit).WithMany(o => o.ProductCapabilis).HasForeignKey(o => o.CapabilitId).OnDelete(DeleteBehavior.Cascade);
    }
}
