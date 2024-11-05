using Wta.Application.Platform;

namespace Wta.Application.Oee;

[Display(Order = -1)]
public class OeeDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
        context.Set<EqpType>().Add(new EqpType { Name = "运行", Number = "10" });
        context.Set<EqpType>().Add(new EqpType { Name = "停机", Number = "20" });
        context.Set<EqpType>().Add(new EqpType { Name = "故障停机", Number = "30" });

        context.Set<EqpCategory>().Add(new EqpCategory { Name = "关机", Number = "10", Type = "20", Color = "#DEDEDE", Remark = "断电", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "离线", Number = "20", Type = "20", Color = "#DEDEDE", Remark = "离线", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "运行", Number = "30", Type = "10", Color = "#29E86C", Remark = "生产运行", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "调机", Number = "40", Type = "20", Color = "#274BDC", Remark = "调机运行", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "空闲", Number = "50", Type = "30", Color = "#FFB852", Remark = "设备停机", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "异常运行", Number = "60", Type = "10", Color = "#9FC944", Remark = "生产运行中存在异常\r\n", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "故障", Number = "70", Type = "30", Color = "#C02828", Remark = "生产设备故障停机\r\n", });
        context.Set<EqpCategory>().Add(new EqpCategory { Name = "待机", Number = "80", Type = "30", Color = "#274BDC", Remark = "", });
        context.SaveChanges();
    }
}
