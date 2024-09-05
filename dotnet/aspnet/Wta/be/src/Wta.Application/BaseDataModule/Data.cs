using Wta.Application.SystemModule.Data;

namespace Wta.Application.BaseDataModule;

public class DataDbConfig : BaseDbConfig<SystemDbContext>,
    IEntityTypeConfiguration<SupplierCategory>,
    IEntityTypeConfiguration<Supplier>,
    IEntityTypeConfiguration<CustomerCategory>,
    IEntityTypeConfiguration<Customer>,
    IEntityTypeConfiguration<ProductType>,
    IEntityTypeConfiguration<ProductCategory>,
    IEntityTypeConfiguration<ProductUnit>,
    IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
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
        builder.HasOne(o => o.Type).WithMany().HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(o => o.Unit).WithMany().HasForeignKey(o => o.TypeId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<ProductUnit> builder)
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

public class Data : IDbSeeder<SystemDbContext>
{
    public void Seed(SystemDbContext context)
    {
        context.Set<Supplier>().Add(new Supplier
        {
            Category = new SupplierCategory
            {
                Name = "默认",
                Number = "Default"
            }.UpdateNode(),
            Name = "测试供应商",
            Number = "0000",
            Contact = "联系人",
            PhoneNumber = "13012345678"
        });
        context.Set<Customer>().Add(new Customer
        {
            Category = new CustomerCategory
            {
                Name = "默认",
                Number = "Default"
            }.UpdateNode(),
            Name = "测试客户",
            Number = "9999",
            Contact = "联系人",
            PhoneNumber = "13012345678"
        });
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
