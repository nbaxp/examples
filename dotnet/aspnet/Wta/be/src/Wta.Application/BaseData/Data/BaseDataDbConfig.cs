using Wta.Application.BaseData.Domain;
using Wta.Application.System.Data;

namespace Wta.Application.BaseData.Data;

public class BaseDataDbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<SupplierCategory>,
    IEntityTypeConfiguration<Supplier>,
    IEntityTypeConfiguration<CustomerCategory>,
    IEntityTypeConfiguration<Customer>,
    IEntityTypeConfiguration<ProductType>,
    IEntityTypeConfiguration<ProductCategory>,
    IEntityTypeConfiguration<Product>,
    IEntityTypeConfiguration<WarehouseCategory>,
    IEntityTypeConfiguration<WarehouseType>,
    IEntityTypeConfiguration<Warehouse>,
    IEntityTypeConfiguration<StorageArea>
{
    public void Configure(EntityTypeBuilder<ProductType> builder)
    {
    }

    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Product> builder)
    {
    }

    public void Configure(EntityTypeBuilder<WarehouseType> builder)
    {
    }

    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
    }

    public void Configure(EntityTypeBuilder<StorageArea> builder)
    {
    }

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

    public void Configure(EntityTypeBuilder<WarehouseCategory> builder)
    {
    }
}
