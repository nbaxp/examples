using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT格式", Order = 0)]
public class IotFormat : BaseNameNumberEntity
{
    [Hidden]
    public List<IotProduct> Products { get; set; } = [];
}
