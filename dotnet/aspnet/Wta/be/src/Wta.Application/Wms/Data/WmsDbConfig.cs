using Wta.Application.System.Data;
using Wta.Application.Wms.Domain;

namespace Wta.Application.Wms.Data;

public class WmsDbConfig : BaseDbConfig<DefaultDbContext>,
    IEntityTypeConfiguration<StorageIn>,
    IEntityTypeConfiguration<StorageOut>,
    IEntityTypeConfiguration<InventoryTransfer>,
    IEntityTypeConfiguration<InventoryCount>

{
    public void Configure(EntityTypeBuilder<StorageIn> builder)
    {
    }

    public void Configure(EntityTypeBuilder<StorageOut> builder)
    {
    }

    public void Configure(EntityTypeBuilder<InventoryTransfer> builder)
    {
    }

    public void Configure(EntityTypeBuilder<InventoryCount> builder)
    {
    }
}
