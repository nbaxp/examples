using Wta.Application.Platform;

namespace Wta.Application.WmsModule;

[DependsOn<PlatformDbContext>, WarehouseManagement, Display(Name = "仓库信息", Order = 10)]
public class WarehouseInfo : Entity
{
}

[DependsOn<PlatformDbContext>, WarehouseManagement, Display(Name = "仓位信息", Order = 20)]
public class WarehouseLocationInfo : Entity
{
}

[DependsOn<PlatformDbContext>, WarehouseManagement, Display(Name = "期初期末", Order = 30)]
public class PeriodStartEnd : Entity
{
}

[DependsOn<PlatformDbContext>, Wms, Display(Name = "其他入库单", Order = 10)]
public class OtherInboundOrder : Entity
{
}

[DependsOn<PlatformDbContext>, Wms, Display(Name = "其他出库单", Order = 20)]
public class OtherOutboundOrder : Entity
{
}

[DependsOn<PlatformDbContext>, Wms, Display(Name = "库存调拨", Order = 30)]
public class InventoryAllocation : Entity
{
}

[DependsOn<PlatformDbContext>, Wms, Display(Name = "库存盘点", Order = 40)]
public class InventoryCheck : Entity
{
}

[DependsOn<PlatformDbContext>, Wms, Display(Name = "出入库统计", Order = 50)]
public class InventoryInOutStatistics : Entity
{
}
