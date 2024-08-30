namespace Wta.Application.WmsModule;

[Display(Name = "仓库管理", Order = 20)]
public class WmsAttribute : GroupAttribute
{
}

[Display(Name = "基础数据", Order = 1)]
public class WmsBaseDataAttribute : WmsAttribute
{
}

[Display(Name = "库存管理", Order = 2)]
public class InventoryAttribute : WmsAttribute
{
}

[Display(Name = "收货管理", Order = 3)]
public class ReceiptManagementAttribute : WmsAttribute
{
}
