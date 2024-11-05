using Wta.Application.BaseData.Domain;
using Wta.Application.Platform;

namespace Wta.Application.BaseData;

[Display(Order = -1)]
public class BaseDataDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
        //计量单位
        var unitCategoryList = new List<UnitCategory>() {
            new() { Name = "数量", Number = "10" },
            new() { Name = "质量", Number = "20" },
            new() { Name = "时间", Number = "30" },
            new() { Name = "长度", Number = "40" },
            new() { Name = "面积", Number = "50" },
            new() { Name = "体积", Number = "60" },
        };

        context.Set<UnitCategory>().AddRange(unitCategoryList);

        var units = new List<Uom>() {
            new(){ Name="件",Number="10",Ratio=1f,CategoryId = unitCategoryList.First(o=>o.Number=="10").Id  },
            new(){ Name="千克",Number="20",Ratio=1f,CategoryId = unitCategoryList.First(o=>o.Number=="20").Id  },
            new(){ Name="小时",Number="30",Ratio=3600f,CategoryId = unitCategoryList.First(o=>o.Number=="30").Id  },
            new(){ Name="天",Number="31",Ratio=86400f,CategoryId = unitCategoryList.First(o=>o.Number=="30").Id  },
            new(){ Name="米",Number="40",Ratio=1f,CategoryId = unitCategoryList.First(o=>o.Number=="40").Id  },
            new(){ Name="平方米",Number="50",Ratio=1f,CategoryId = unitCategoryList.First(o=>o.Number=="50").Id  },
            new(){ Name="立方米",Number="60",Ratio=1f,CategoryId = unitCategoryList.First(o=>o.Number=="60").Id  },
        };

        context.Set<Uom>().AddRange(units);

        //工位
        var workstationCategoryList = new List<WorkstationCategory>() {
            new() {Id = context.NewGuid(), Name = "厂区", Number = "10" },
            new() {Id = context.NewGuid(), Name = "车间", Number = "20" },
            new() {Id = context.NewGuid(), Name = "产线", Number = "30" },
            new() {Id = context.NewGuid(), Name = "线体", Number = "40" },
            new() {Id = context.NewGuid(), Name = "工位", Number = "50" },
        };

        context.Set<WorkstationCategory>().AddRange(workstationCategoryList);

        context.Set<Workstation>().Add(new Workstation()
        {
            Name = "厂区1",
            Number = "0100000000",
            CategoryId = workstationCategoryList.First(o => o.Number == "10").Id,
            Children = [
                new()
                {
                    Name = "车间1",
                    Number = "0101000000",
                    CategoryId = workstationCategoryList.First(o => o.Number == "20").Id,
                    Children = [
                        new()
                        {
                            Name = "产线1",
                            Number = "0101010000",
                            CategoryId = workstationCategoryList.First(o => o.Number == "30").Id,
                            Children = [
                                new()
                                {
                                    Name = "线体1",
                                    Number = "0101010100",
                                    CategoryId = workstationCategoryList.First(o => o.Number == "40").Id,
                                    Children = [
                                        new()
                                        {
                                            Name = "工位1",
                                            Number = "0101010101",
                                            CategoryId = workstationCategoryList.First(o => o.Number == "50").Id,
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
            ]
        }.UpdateNode());

        //资产
        var assetCategoryList = new List<AssetCategory>(){
            new() {Id = context.NewGuid(), Name = "生产设备", Number = "10" },
            new() {Id = context.NewGuid(), Name = "其他设备", Number = "20" },
        };
        context.Set<AssetCategory>().AddRange(assetCategoryList);
        var assets = new List<Asset>() {
            new Asset
            {
                Name = "生产设备1",
                Number = "01",
                CategoryId = assetCategoryList.First(o => o.Number == "10").Id,
                Values = [new KeyValue { Key="key",Value="value" }]
            }.UpdateNode(),
            new Asset
            {
                Name = "生产设备2",
                Number = "02",
                CategoryId = assetCategoryList.First(o => o.Number == "10").Id }.UpdateNode()
            };
        context.Set<Asset>().AddRange(assets);
        //
        var workstationStatus = new List<WorkstationStatus> {
            new(){ Name = "呼叫主管", Number = "10" },
            new(){ Name = "呼叫维修班长", Number = "20" },
            new(){ Name = "呼叫维修", Number = "30" },
            new(){ Name = "部品异常", Number = "40" },
            new(){ Name = "呼叫班长", Number = "50" },
            new(){ Name = "机种交换", Number = "60" },
            new(){ Name = "料把回收", Number = "70" },
            new(){ Name = "工艺参数错误", Number = "80" },
        };
        context.Set<WorkstationStatus>().AddRange(workstationStatus);
        //
        context.SaveChanges();
    }
}
