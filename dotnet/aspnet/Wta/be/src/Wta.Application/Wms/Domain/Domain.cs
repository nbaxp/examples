namespace Wta.Application.Wms.Domain;

[Inventory, Display(Name = "入库单", Order = 1)]
public class StorageIn : Entity
{
}

[Inventory, Display(Name = "出库单", Order = 2)]
public class StorageOut : Entity
{
}

[Inventory, Display(Name = "库存调拨", Order = 3)]
public class InventoryTransfer : Entity
{
}

[Inventory, Display(Name = "库存盘点", Order = 4)]
public class InventoryCount : Entity
{
}
