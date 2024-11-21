using Wta.Application.Oee.Domain;
using Wta.Application.Platform;

namespace Wta.Application.Oee;

[Display(Order = -1)]
public class OeeDbSeeder() : BaseDbSeeder<PlatformDbContext>
{
    public override void Seed(PlatformDbContext context)
    {
        //OeeStatusType
        context.Set<OeeStatusType>().Add(new() { Number = "T100", Name = "正常生产" });
        context.Set<OeeStatusType>().Add(new() { Number = "T200", Name = "计划停产" });
        context.Set<OeeStatusType>().Add(new() { Number = "T310", Name = "设备调整停产" });
        context.Set<OeeStatusType>().Add(new() { Number = "T320", Name = "设备故障停产" });
        context.Set<OeeStatusType>().Add(new() { Number = "T330", Name = "非设备因素停产" });
        context.SaveChanges();
        //OeeStatus
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "正常生产"),Color= "#009900", Number = "S100", Name = "正常生产" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "计划停产"), Color = "#0066FF", Number = "S210", Name = "休息" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "计划停产"), Color = "#0066FF", Number = "S220", Name = "自检" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "计划停产"), Color = "#0066FF", Number = "S230", Name = "计划停产" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "设备调整停产"), Color = "#FF9900", Number = "S310", Name = "设备调整停产" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "设备故障停产"), Color = "#FF0000", Number = "S320", Name = "设备故障停产" });
        context.Set<OeeStatus>().Add(new() { TypeId = FindIdByName<OeeStatusType>(context, "非设备因素停产"), Color = "#0FF000009900", Number = "S330", Name = "非设备因素停产" });
        context.SaveChanges();
        //OeeFaultReason
        context.Set<OeeReason>().Add(new() { Number = "R100", Name = "电机故障" });
        context.Set<OeeReason>().Add(new() { Number = "R200", Name = "材料异常" });
        context.Set<OeeReason>().Add(new() { Number = "R300", Name = "电气异常" });
        context.SaveChanges();
        //OeeActionCategory
        context.Set<OeeActionCategory>().Add(new() { Number = "Environment", Name = "环境" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Equipment", Name = "设备" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Material", Name = "材料" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Measurement", Name = "计量" });
        context.Set<OeeActionCategory>().Add(new() { Number = "People", Name = "人员" });
        context.Set<OeeActionCategory>().Add(new() { Number = "Process", Name = "程序" });
        context.SaveChanges();
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
        SetData(context, DateTime.Now.AddDays(-4));
        SetData(context, DateTime.Now.AddDays(-3));
        SetData(context, DateTime.Now.AddDays(-2));
        SetData(context, DateTime.Now.AddDays(-1));
        SetData(context, DateTime.Now);
        context.SaveChanges();
    }

    private void SetData(PlatformDbContext context, DateTime day)
    {
        var shfitNumber = context.Set<OeeShift>().First().Number;
        var assetNumber = context.Set<OeeAsset>().First(o => o.Number == "01010101").Number;
        var partNumber = context.Set<OeePart>().First().Number;
        var standardUpm = context.Set<OeePart>().First().StandardUpm;
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftId = FindIdByName<OeeShift>(context, "白班"),
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(8),
            End = DateTime.Now.Date.AddHours(10),
            Duration = (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now.Date.AddHours(8)).TotalMinutes,
            StandardUpm = standardUpm,
            SpeedUpm = 0.5f,
            Total = 180,
            EequipmentScrap = 10,
            StatusId = FindIdByName<OeeStatus>(context, "正常生产"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftId = FindIdByName<OeeShift>(context, "白班"),
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(10),
            End = DateTime.Now.Date.AddHours(11),
            Duration = (int)(DateTime.Now.Date.AddHours(11) - DateTime.Now.Date.AddHours(10)).TotalMinutes,
            StatusId = FindIdByName<OeeStatus>(context, "计划停产"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftId = FindIdByName<OeeShift>(context, "白班"),
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(11),
            End = DateTime.Now.Date.AddHours(11.5),
            Duration = (int)(DateTime.Now.Date.AddHours(11.5) - DateTime.Now.Date.AddHours(11)).TotalMinutes,
            StatusId = FindIdByName<OeeStatus>(context, "设备调整停产"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftId = FindIdByName<OeeShift>(context, "晚班"),
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(11.5),
            End = DateTime.Now.Date.AddHours(12),
            Duration = (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now.Date.AddHours(11.5)).TotalMinutes,
            StatusId = FindIdByName<OeeStatus>(context, "设备故障停产"),
            Operator = "admin",
        });
        context.Set<OeeData>().Add(new OeeData()
        {
            Date = DateOnly.FromDateTime(day),
            ShiftId = FindIdByName<OeeShift>(context, "晚班"),
            AssetNumber = assetNumber,
            PartNumber = partNumber,
            Start = DateTime.Now.Date.AddHours(11.5),
            End = DateTime.Now.Date.AddHours(12),
            Duration = (int)(DateTime.Now.Date.AddHours(12) - DateTime.Now.Date.AddHours(11.5)).TotalMinutes,
            StatusId = FindIdByName<OeeStatus>(context, "非设备因素停产"),
            Operator = "admin",
        });
    }
}
