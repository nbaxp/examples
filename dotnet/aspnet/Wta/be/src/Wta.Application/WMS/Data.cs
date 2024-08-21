using Wta.Application.System.Data;

namespace Wta.Application.WMS;

public class DbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<LocationType>,
    IEntityTypeConfiguration<StorageLocation>,
    IEntityTypeConfiguration<InventoryOperation>,
    IEntityTypeConfiguration<Inventory>,
    IEntityTypeConfiguration<InventoryTransaction>

{
    public void Configure(EntityTypeBuilder<LocationType> builder)
    {
    }

    public void Configure(EntityTypeBuilder<StorageLocation> builder)
    {
        builder.HasOne(o => o.Type).WithMany(o => o.Locations).HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.HasOne(o => o.Location).WithMany(o => o.Inventories).HasForeignKey(o => o.LocationId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasOne(o => o.Operation).WithMany(o => o.Transactions).HasForeignKey(o => o.OperationId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(o => o.Location).WithMany(o => o.Transactions).HasForeignKey(o => o.LocationId).OnDelete(DeleteBehavior.SetNull);
    }

    public void Configure(EntityTypeBuilder<InventoryOperation> builder)
    {
    }
}

public class DataDbSeeder : IDbSeeder<DefaultDbContext>
{
    public void Seed(DefaultDbContext context)
    {
        //库位类型
        context.Set<LocationType>().AddRange([
            new() {Id=context.NewGuid(), Name = "公司", Number = "company" },
            new() {Id=context.NewGuid(), Name = "工厂", Number = "factory" },
            new() {Id=context.NewGuid(), Name = "仓库", Number = "warehouse" },
            new() {Id=context.NewGuid(), Name = "库位", Number = "location" },
        ]);
        context.SaveChanges();
        //库存操作
        context.Set<InventoryOperation>().AddRange([
            new() {Id=context.NewGuid(), Name = "采购入库", Number = "10",Direction = InventoryDirection.In },
            new() {Id=context.NewGuid(), Name = "客户退货入库", Number = "20",Direction = InventoryDirection.In },
            new() {Id=context.NewGuid(), Name = "生产入库", Number = "30",Direction = InventoryDirection.In },
            new() {Id=context.NewGuid(), Name = "退料入库", Number = "40",Direction = InventoryDirection.In },
            new() {Id=context.NewGuid(), Name = "其他入库", Number = "50",Direction = InventoryDirection.In },
            new() {Id=context.NewGuid(), Name = "销售出库", Number = "60",Direction = InventoryDirection.Out },
            new() {Id=context.NewGuid(), Name = "领料出库", Number = "70",Direction = InventoryDirection.Out },
            new() {Id=context.NewGuid(), Name = "采购退货出库", Number = "80",Direction = InventoryDirection.Out },
            new() {Id=context.NewGuid(), Name = "其他出库", Number = "90",Direction = InventoryDirection.Out },

        ]);
        context.SaveChanges();
        //库位
        context.Set<StorageLocation>().AddRange([
            new StorageLocation{
                Id=context.NewGuid(),
                Name ="工厂1",
                Number="factory1",
                TypeId = context.Set<LocationType>().First(o=>o.Number=="factory").Id,
                Children = [
                    new (){
                        Id=context.NewGuid(),
                        Name ="仓库1",
                        Number="warehouse1",
                        TypeId = context.Set<LocationType>().First(o=>o.Number=="warehouse").Id,
                        Children = [
                            new (){
                                Id=context.NewGuid(),
                                Name ="库位1",
                                Number="location1",
                                TypeId = context.Set<LocationType>().First(o=>o.Number=="location").Id,
                            }
                        ]
                    }
                ]
            }.UpdateNode(),
            new StorageLocation{
                Id=context.NewGuid(),
                Name ="工厂2",
                Number="factory2",
                TypeId = context.Set<LocationType>().First(o=>o.Number=="factory").Id,
                Children = [
                    new (){
                        Id=context.NewGuid(),
                        Name ="仓库2",
                        Number="warehouse2",
                        TypeId = context.Set<LocationType>().First(o=>o.Number=="warehouse").Id,
                        Children = [
                            new (){
                                Id=context.NewGuid(),
                                Name ="库位2",
                                Number="location2",
                                TypeId = context.Set<LocationType>().First(o=>o.Number=="location").Id,
                            }
                        ]
                    }
                ]
            }.UpdateNode(),
        ]);
        context.SaveChanges();
        //库存
        var id = context.Set<StorageLocation>().FirstOrDefault(o => o.Number == "location1")?.Id;
        var id2 = context.Set<StorageLocation>().First(o => o.Number == "location1")?.Id;
        context.Set<Inventory>().AddRange([
            new (){
                Id=context.NewGuid(),
                Name="原材料1",
                Number="001",
                Quantity = 50,
                LocationId =context.Set<StorageLocation>().First(o=>o.Number=="location1").Id,
            }
        ]);
        context.SaveChanges();
        //库存事务
        context.Set<InventoryTransaction>().AddRange([
            new (){
                Id=context.NewGuid(),
                Name="原材料1",
                Number="001",
                Quantity = 100,
                Direction = InventoryDirection.In,
                LocationId =context.Set<StorageLocation>().First(o=>o.Number=="location1").Id,
                OperationId=context.Set<InventoryOperation>().First(o=>o.Number=="10").Id
            },
            new (){
                Id=context.NewGuid(),
                Name="原材料1",
                Number="002",
                Quantity = 50,
                Direction = InventoryDirection.Out,
                LocationId =context.Set<StorageLocation>().First(o=>o.Number=="location1").Id,
                OperationId=context.Set<InventoryOperation>().First(o=>o.Number=="70").Id
            },
        ]);
    }
}
