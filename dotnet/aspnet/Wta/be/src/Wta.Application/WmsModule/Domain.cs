using Wta.Application.SystemModule.Data;

namespace Wta.Application.WmsModule;

[DependsOn<SystemDbContext>, WareHouseManagement, Display(Name = "仓库信息", Order = 10)]
public class WareHouseInfo : Entity
{
}

[DependsOn<SystemDbContext>, WareHouseManagement, Display(Name = "仓位信息", Order = 20)]
public class WarehouseLocationInfo : Entity
{
}

[DependsOn<SystemDbContext>, WareHouseManagement, Display(Name = "期初期末", Order = 30)]
public class PeriodStartEnd : Entity
{
}

[DependsOn<SystemDbContext>, Wms, Display(Name = "其他入库单", Order = 10)]
public class OtherInboundOrder : Entity
{
}

[DependsOn<SystemDbContext>, Wms, Display(Name = "其他出库单", Order = 20)]
public class OtherOutboundOrder : Entity
{
}

[DependsOn<SystemDbContext>, Wms, Display(Name = "库存调拨", Order = 30)]
public class InventoryAllocation : Entity
{
}

[DependsOn<SystemDbContext>, Wms, Display(Name = "库存盘点", Order = 40)]
public class InventoryCheck : Entity
{
}

[DependsOn<SystemDbContext>, Wms, Display(Name = "出入库统计", Order = 50)]
public class InventoryInOutStatistics : Entity
{
}
