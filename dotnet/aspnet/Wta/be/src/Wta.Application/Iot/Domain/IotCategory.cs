using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT产品分类", Order = 20)]
public class IotCategory : BaseTreeEntity<IotCategory>
{
    [Hidden]
    public List<IotProduct> Products { get; set; } = [];
}
