using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.BaseData.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT设备", Order = 40)]
public class IotDevice : BaseNameNumberEntity, IEntityTypeConfiguration<IotDevice>
{
    [UIHint("select")]
    [KeyValue("url", "iot-product/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "产品")]
    public Guid ProductId { get; set; }

    public IotProduct? Product { get; set; }

    [Hidden]
    public List<KeyValue> Values { get; set; } = [];

    [NotMapped]
    [Display(Name = "属性")]
    public List<string> FormatValues
    {
        get
        {
            var result = new List<string>();
            if (this.Product?.ProductCapabilis != null)
            {
                foreach (var item in this.Product.ProductCapabilis)
                {
                    var value = Values.FirstOrDefault(v => v.Key == item.Capabilit?.Number)?.Value?.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        var kv = $"{item.Capabilit?.Name!}:{value}";
                        if (!string.IsNullOrEmpty(item.Capabilit?.Unit))
                        {
                            kv += $" {item.Capabilit?.Unit}";
                        }
                        result.Add(kv);
                    }
                }
            }
            return result;
        }
    }

    public void Configure(EntityTypeBuilder<IotDevice> builder)
    {
        builder.HasOne(o => o.Product).WithMany(o => o.Devices).HasForeignKey(o => o.ProductId).OnDelete(DeleteBehavior.Cascade);
        builder.OwnsMany(x => x.Values, o => o.ToJson());
    }
}
