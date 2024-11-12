using Wta.Application.Oee.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Oee;

[Display(Order = -1)]
public class OeeDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
        //EventReanon
        context.Set<OeeReason>().Add(new OeeReason()
        {
            Number = "Planned downtimes",
            Name = "计划停机",
            Children = new List<OeeReason>
            {
                new() { Number="C001",Name="Changeover(15 min)" },
                new() { Number="P001",Name="Lunch Break" },
                new() { Number="P002",Name="Shift breaks" },
                new() { Number="P003",Name="Planned not to run" },
            }
        }.UpdateNode());
        context.Set<OeeReason>().Add(new OeeReason
        {
            Number = "Unplanned downtimes",
            Name = "非计划停机",
            Children = new List<OeeReason>
            {
                new(){
                    Number="Logistics issue",
                    Name="物流问题",
                    Children = new List<OeeReason>{
                        new() { Number="C002",Name="Changeover Overrun" },
                        new() { Number="L001",Name="Line side Inventory short" },
                        new() { Number="L002",Name="Packing Line Overfull" },
                    }
                },
                new(){
                    Number="Engineering Issues",
                    Name="技术问题",
                    Children= new List<OeeReason>{
                        new() { Number="E001",Name="Changeover(15 min)" },
                        new() { Number="E002",Name="Packing Line broken" },
                        new() { Number="E003",Name="Belt feed issues" },
                    }
                },
            }
        }.UpdateNode());
        //OeeAsset
        context.Set<OeeAsset>().Add(new OeeAsset
        {
            Name = "车间1",
            Number = "01",
            Children = new List<OeeAsset>
            {
                new OeeAsset
                {
                    Name="产线1",
                    Number="0101",
                    Children = new List<OeeAsset>
                    {
                        new OeeAsset
                        {
                            Name="工位1",
                            Number="010101",
                            Children = new List<OeeAsset>
                            {
                                new OeeAsset
                                {
                                    Name="设备1",
                                    Number="01010101",
                                }
                            }
                        }
                    }
                }
            },
            Shifts = new List<OeeShift>
            {
                new OeeShift{
                    Name="白班",
                    Number="day",
                    Start=new TimeOnly(8,30),
                    End=new TimeOnly(16,30),
                },
                new OeeShift{
                    Name="晚班",
                    Number="night",
                    Start=new TimeOnly(17,30),
                    End=new TimeOnly(20,30),
                }
            }
        }.UpdateNode());
        context.SaveChanges();
    }
}
