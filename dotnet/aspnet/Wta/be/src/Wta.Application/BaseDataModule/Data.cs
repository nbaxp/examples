using Wta.Application.BaseModule.Data;

namespace Wta.Application.BaseDataModule;

public class DataDbConfig : BaseDbConfig<BaseDbContext>,
    IEntityTypeConfiguration<SupplierCategory>,
    IEntityTypeConfiguration<Supplier>,
    IEntityTypeConfiguration<CustomerCategory>,
    IEntityTypeConfiguration<Customer>,
    IEntityTypeConfiguration<ProductType>,
    IEntityTypeConfiguration<ProductCategory>,
    IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
    }

    public void Configure(EntityTypeBuilder<SupplierCategory> builder)
    {
    }

    public void Configure(EntityTypeBuilder<CustomerCategory> builder)
    {
    }

    //public void Configure(EntityTypeBuilder<WarehouseCategory> builder)
    //{
    //}

    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
    }

    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Product> builder)
    {
    }

    //public void Configure(EntityTypeBuilder<WarehouseType> builder)
    //{
    //}

    //public void Configure(EntityTypeBuilder<Warehouse> builder)
    //{
    //}

    //public void Configure(EntityTypeBuilder<StorageArea> builder)
    //{
    //}
}


public class Data : IDbSeeder<BaseDbContext>
{
    public void Seed(BaseDbContext context)
    {
        ////产品类型
        //context.Set<ProductType>().AddRange([new ProductType()
        //{
        //    Id = context.NewGuid(),
        //    Name = "主料",
        //    Number = "01"
        //}.UpdateNode(),
        //    new ProductType()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "辅料",
        //        Number = "02"
        //    }.UpdateNode()]);

        ////产品分类
        //context.Set<ProductCategory>().AddRange([new ProductCategory()
        //{
        //    Id = context.NewGuid(),
        //    Name = "CPU芯片",
        //    Number = "01"
        //}.UpdateNode(),
        //    new ProductCategory()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "散热风扇",
        //        Number = "02"
        //    }.UpdateNode()]);

        ////仓库类型
        //context.Set<WarehouseType>().AddRange([new WarehouseType()
        //{
        //    Id = context.NewGuid(),
        //    Name = "存货仓",
        //    Number = "01"
        //}.UpdateNode(),
        //    new WarehouseType()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "现场仓",
        //        Number = "02"
        //    }.UpdateNode(),
        //    new WarehouseType()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "废品仓",
        //        Number = "03"
        //    }.UpdateNode()]);

        ////仓库
        //context.Set<Warehouse>().AddRange([new Warehouse()
        //{
        //    Id = context.NewGuid(),
        //    Name = "杭州仓",
        //    Number = "01"
        //}.UpdateNode(),
        //    new Warehouse()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "无锡仓",
        //        Number = "02"
        //    }.UpdateNode()]);

        ////库位

        ////仓库
        //context.Set<Warehouse>().AddRange([new Warehouse()
        //{
        //    Id = context.NewGuid(),
        //    Name = "杭州仓01",
        //    Number = "01"
        //}.UpdateNode(),
        //    new Warehouse()
        //    {
        //        Id = context.NewGuid(),
        //        Name = "无锡仓02",
        //        Number = "02"
        //    }.UpdateNode()]);

        //保存
        context.SaveChanges();
    }
}
