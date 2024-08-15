namespace Wta.Application.Wms;

[Display(Name = "WMS", Order = 20)]
public class WmsGroupAttribute : GroupAttribute
{
}

[Display(Name = "仓库管理", Order = 1)]
public class WarehouseAttribute : WmsGroupAttribute
{
}

[Display(Name = "库存管理", Order = 2)]
public class InventoryAttribute : WmsGroupAttribute
{
}
