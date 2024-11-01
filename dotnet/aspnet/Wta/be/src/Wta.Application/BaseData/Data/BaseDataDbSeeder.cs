using Wta.Application.BaseData.Domain;
using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Data;

[Display(Order = -1)]
public class BaseDataDbSeeder() : IDbSeeder<PlatformDbContext>
{
    public void Seed(PlatformDbContext context)
    {
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
                        new ()
                        {
                            Name = "产线1",
                            Number = "0101010000",
                            CategoryId = workstationCategoryList.First(o => o.Number == "30").Id,
                            Children = [
                                new ()
                                {
                                    Name = "线体1",
                                    Number = "0101010100",
                                    CategoryId = workstationCategoryList.First(o => o.Number == "40").Id,
                                    Children = [
                                        new ()
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
        //
        var assetCategoryList = new List<AssetCategory>(){
            new() {Id = context.NewGuid(), Name = "生产设备", Number = "10" },
            new() {Id = context.NewGuid(), Name = "其他设备", Number = "20" },
        };
        context.Set<AssetCategory>().AddRange(assetCategoryList);
        var assets = new List<Asset>() { new Asset { Name = "生产设备1", Number = "01", CategoryId = assetCategoryList.First(o => o.Number == "10").Id }.UpdateNode(),
        new Asset { Name = "生产设备2", Number = "02", CategoryId = assetCategoryList.First(o => o.Number == "10").Id }.UpdateNode()};
        context.Set<Asset>().AddRange(assets);
        //
        context.SaveChanges();
    }
}
