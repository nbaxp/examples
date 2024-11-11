using DocumentFormat.OpenXml.Wordprocessing;
using Wta.Application.Platform;

namespace Wta.Application.Iot.Domain;

[Iot]
[DependsOn<PlatformDbContext>]
[Display(Name = "IoT产品功能", Order = 10)]
public class IotCapabilit : BaseNameNumberEntity
{
    [Display(Name = "功能类型")]
    public IotCapabilitType Type { get; set; }

    [Display(Name = "数据类型")]
    public DataType? DataType { get; set; }

    [Display(Name = "数据单位")]
    public string? Unit { get; set; }

    [Display(Name = "默认值")]
    public string? Default { get; set; }

    [Hidden]
    public List<IotProductCapabilit> ProductCapabilis { get; set; } = [];
}

public enum IotCapabilitType
{
    [Display(Name = "静态属性")]
    Static = 10,

    [Display(Name = "数据属性")]
    Data = 20,

    [Display(Name = "操作接口")]
    Api = 30
}

public enum DataType
{
    Int,
    Float,
    String,
    Date,
    DateTime
}
