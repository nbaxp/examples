namespace Wta.Application.System.Domain;

[Display(Name = "WMS", Order = 1)]
public class WmsAttribute : GroupAttribute
{
}

[Display(Name = "仓库管理", Order = 1)]
public class WarehouseAttribute : WmsAttribute
{
}

[Display(Name = "库存管理", Order = 2)]
public class InventoryAttribute : WmsAttribute
{
}
