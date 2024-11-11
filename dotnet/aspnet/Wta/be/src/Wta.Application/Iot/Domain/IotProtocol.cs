using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT协议", Order = 0)]
public class IotProtocol : BaseNameNumberEntity
{
    [Display(Name = "服务器")]
    public string Server { get; set; } = default!;
    [Hidden]
    public List<IotProduct> Products { get; set; } = [];
}
