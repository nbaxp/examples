using Wta.Application.BaseData.Domain;
using Wta.Application.Iot.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Iot;

[Display(Order = -1)]
public class IotDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
        context.Set<IotProtocol>().Add(new IotProtocol
        {
            Name = "MQTT",
            Number = "mqtt",
            Server = "mqtt://admin:aA123456@localhost:1883"
        });
        context.Set<IotFormat>().Add(new IotFormat
        {
            Name = "JSON",
            Number = "json"
        });
        context.SaveChanges();
        context.Set<IotCapabilit>().Add(new IotCapabilit
        {
            Type = IotCapabilitType.Static,
            Name = "测试参数",
            Number = "test"
        });
        context.SaveChanges();
        context.Set<IotCategory>().Add(new IotCategory
        {
            Name = "测试分类",
            Number = "10",
        }.UpdateNode());
        context.SaveChanges();
        context.Set<IotProduct>().Add(new IotProduct
        {
            Name = "测试产品",
            Number = "10",
            CategoryId = context.Set<IotCategory>().First().Id,
            ProtocolId = context.Set<IotProtocol>().First().Id,
            FormatId = context.Set<IotFormat>().First().Id,
        });
        context.SaveChanges();
        context.Set<IotProductCapabilit>().Add(new IotProductCapabilit
        {
            ProductId = context.Set<IotProduct>().First().Id,
            CapabilitId = context.Set<IotCapabilit>().First().Id,
        });
        context.SaveChanges();
        context.Set<IotDevice>().Add(new IotDevice
        {
            Name = "测试设备",
            Number = "10",
            ProductId = context.Set<IotProduct>().First().Id,
            Values = [
                new KeyValue
                {
                    Key ="test",
                    Value="123"
                }
            ]
        });
    }
}
