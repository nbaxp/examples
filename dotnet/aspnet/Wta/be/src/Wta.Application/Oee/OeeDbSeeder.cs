using Wta.Application.Oee.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Oee;

[Display(Order = -1)]
public class OeeDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
        //OeeActionCategory
        context.Set<OeeActionCategory>().Add(new() { Number = "Environment", Name = "Environment" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Equipment", Name = "Equipment" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Material", Name = "Material" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Measurement", Name = "Measurement" });
        context.Set<OeeActionCategory>().Add(new() { Number = "People", Name = "People" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Process", Name = "Process" });
        context.SaveChanges();
        //OeeRequirement
        context.Set<OeeActionStatus>().Add(new() { Number = "Describe", Name = "Describe" });
        context.Set<OeeActionStatus>().Add(new() { Number = "Investigate", Name = "Investigate" });
        context.Set<OeeActionStatus>().Add(new() { Number = "Identify Countermeasure", Name = "Identify Countermeasure" });
        context.Set<OeeActionStatus>().Add(new() { Number = "Implementation", Name = "Implementation" });
        context.Set<OeeActionStatus>().Add(new() { Number = "Review", Name = "Review" });
        context.SaveChanges();
        //EventReanon
        context.Set<OeeStatus>().Add(new OeeStatus()
        {
            Type = OeeStatusType.PlannedDowntime,
            Number = "PlannedDowntime",
            Name = "计划停机",
            Children =
            [
                new()
                {
                    Number="S001",
                    Name="休息时间"
                },
                new()
                {
                    Number="S002",
                    Name="日常管理"
                },
                new()
                {
                    Number="S003",
                    Name="计划停机"
                },
            ]
        }.UpdateNode());
        context.Set<OeeStatus>().Add(new OeeStatus()
        {
            Type = OeeStatusType.ProductionTime,
            Number = "S004",
            Name = "正常生产",
        }.UpdateNode());
        context.Set<OeeStatus>().Add(new OeeStatus()
        {
            Type = OeeStatusType.UnloadedTime,
            Number = "S005",
            Name = "无负荷运作",
        }.UpdateNode());
        context.Set<OeeStatus>().Add(new OeeStatus
        {
            Type = OeeStatusType.UnplannedDowntime,
            Number = "UnplannedDowntime",
            Name = "非计划停机",
            Children = new List<OeeStatus>
            {
                new(){
                    Number="S006",
                    Name="物流问题",
                },
                new(){
                    Number="S007",
                    Name="技术问题",
                },
            }
        }.UpdateNode());
        //OeePart
        context.Set<OeePart>().Add(new OeePart { Name = "Acme001", Number = "Acme001", OpCode = "OP_001", StandardUpm = 1f });
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
                    End=new TimeOnly(22,30),
                }
            }
        }.UpdateNode());
        context.SaveChanges();
        //data
        SetData(context, DateTime.Now.AddDays(-2));
        SetData(context, DateTime.Now.AddDays(-1));
        SetData(context, DateTime.Now);
        context.SaveChanges();
    }

    private static void SetData(PlatformDbContext context, DateTime day)
    {
        var shfitNumber = context.Set<OeeShift>().First().Number;
        var assetNumber = context.Set<OeeAsset>().First(o => o.Number == "01010101").Number;
        var partNumber = context.Set<OeePart>().First().Number;
        var standardUpm = context.Set<OeePart>().First().StandardUpm;
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftNumber = shfitNumber,
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(8),
            End = DateTime.Now.Date.AddHours(10),
            Duration = (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now.Date.AddHours(8)).TotalMinutes,
            StandardUpm = standardUpm,
            SpeedUpm = 0.5f,
            TotalItems = 180,
            ScrapItems = 10,
            StatusNumber = GetStatus(context, "正常生产"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftNumber = shfitNumber,
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(10),
            End = DateTime.Now.Date.AddHours(11),
            Duration = (int)(DateTime.Now.Date.AddHours(11) - DateTime.Now.Date.AddHours(10)).TotalMinutes,
            StatusNumber = GetStatus(context, "休息时间"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftNumber = shfitNumber,
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(11),
            End = DateTime.Now.Date.AddHours(11.5),
            Duration = (int)(DateTime.Now.Date.AddHours(11.5) - DateTime.Now.Date.AddHours(11)).TotalMinutes,
            StatusNumber = GetStatus(context, "物流问题"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftNumber = shfitNumber,
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(11.5),
            End = DateTime.Now.Date.AddHours(12),
            Duration = (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now.Date.AddHours(11.5)).TotalMinutes,
            StatusNumber = GetStatus(context, "无负荷运作"),
            Operator = "admin",
        });
    }

    private static string GetStatus(PlatformDbContext context, string name)
    {
        return context.Set<OeeStatus>().First(o => o.Name == name).Number;
    }
}
