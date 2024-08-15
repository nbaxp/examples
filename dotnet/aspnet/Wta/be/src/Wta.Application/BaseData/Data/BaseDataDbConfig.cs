using Wta.Application.BaseData.Domain;
using Wta.Application.System.Data;

namespace Wta.Application.BaseData.Data;

public class BaseDataDbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<Supplier>,
    IEntityTypeConfiguration<Customer>,
    IEntityTypeConfiguration<ProductType>,
    IEntityTypeConfiguration<ProductCategory>,
    IEntityTypeConfiguration<Product>,
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
}
