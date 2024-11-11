using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT产品", Order = 30)]
public class IotProduct : BaseNameNumberEntity, IEntityTypeConfiguration<IotProduct>
{
    [UIHint("select")]
    [KeyValue("url", "iot-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "分类")]
    public Guid? CategoryId { get; set; }

    public IotCategory? Category { get; set; }

    [UIHint("select")]
    [KeyValue("url", "iot-protocol/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "协议")]
    public Guid? ProtocolId { get; set; }

    public IotProtocol? Protocol { get; set; }

    [UIHint("select")]
    [KeyValue("url", "iot-format/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "协议")]
    public Guid? FormatId { get; set; }

    public IotFormat? Format { get; set; }

    [Hidden]
    public List<IotDevice> Devices { get; set; } = [];

    [Hidden]
    public List<IotProductCapabilit> ProductCapabilis { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "iot-capabilit/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [NotMapped]
    [Display(Name = "产品功能")]
    public List<Guid> Capabilits
    {
        get
        {
            return this.ProductCapabilis?.Select(o => o.CapabilitId).ToList() ?? [];
        }
        set
        {
            this.ProductCapabilis = value?.Distinct().Select(o => new IotProductCapabilit { CapabilitId = o }).ToList() ?? [];
        }
    }

    public void Configure(EntityTypeBuilder<IotProduct> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Products).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Protocol).WithMany(o => o.Products).HasForeignKey(o => o.ProtocolId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Format).WithMany(o => o.Products).HasForeignKey(o => o.FormatId).OnDelete(DeleteBehavior.Cascade);
    }
}
