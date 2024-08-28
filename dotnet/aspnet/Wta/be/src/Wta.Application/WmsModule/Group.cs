namespace Wta.Application.Wms;

[Display(Name = "仓库管理", Order = 20)]
public class WmsAttribute : GroupAttribute
{
}

[Display(Name = "基础数据", Order = 1)]
public class BaseDataAttribute : WmsAttribute
{
}

[Display(Name = "库存管理", Order = 2)]
public class InventoryAttribute : WmsAttribute
{
}
