namespace Wta.Application.WmsModule;

[WmsBaseData, Display(Name = "库位类型", Order = 1)]
public class LocationType : BaseNameNumberEntity
{
    [Hidden]
    public List<StorageLocation> Locations { get; set; } = [];
}

[WmsBaseData, Display(Name = "库位管理", Order = 2)]
public class StorageLocation : BaseTreeEntity<StorageLocation>
{
    [UIHint("select")]
    [KeyValue("url", "location-type/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "类型")]
    [Required]
    public Guid? TypeId { get; set; }

    public LocationType? Type { get; set; }

    public List<InventoryTransaction> Transactions { get; set; } = [];
    public List<Inventory> Inventories { get; set; } = [];
}

public enum InventoryDirection
{
    [Display(Name = "入库")]
    In,

    [Display(Name = "出库")]
    Out
}

[WmsBaseData, Display(Name = "库存操作", Order = 3)]
public class InventoryOperation : BaseNameNumberEntity
{
    [Display(Name = "操作类型")]
    public InventoryDirection Direction { get; set; }

    [Hidden]
    public List<InventoryTransaction> Transactions { get; set; } = [];
}

[Inventory, Display(Name = "库存管理", Order = 1)]
public class Inventory : BaseNameNumberEntity
{
    [Display(Name = "数量")]
    public int Quantity { get; internal set; }

    [UIHint("select")]
    [KeyValue("url", "storage-location/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "库位")]
    [Required]
    public Guid? LocationId { get; internal set; }

    [Hidden]
    public StorageLocation? Location { get; set; }
}

[Inventory, Display(Name = "库存事务", Order = 2)]
public class InventoryTransaction : BaseNameNumberEntity
{
    [Display(Name = "来源")]
    public string? From { get; set; }

    [Display(Name = "目标")]
    public string? To { get; set; }

    [Display(Name = "分组")]
    public string? Group { get; set; }

    [Display(Name = "操作类型")]
    public InventoryDirection Direction { get; set; }

    [UIHint("select")]
    [KeyValue("url", "inventory-operation/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "操作")]
    [Required]
    public Guid? OperationId { get; set; }

    [Hidden]
    public InventoryOperation? Operation { get; set; }

    [UIHint("select")]
    [KeyValue("url", "storage-location/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "库位")]
    [Required] public Guid? LocationId { get; internal set; }

    [Display(Name = "数量")]
    public int Quantity { get; internal set; }

    [Hidden]
    public StorageLocation? Location { get; set; }

    [Hidden]
    public List<InventoryTransaction> Transactiona { get; set; } = [];
}

public enum WmsAsnStatus
{
    [Display(Name = "待到货")]
    Arrive = 0,

    [Display(Name = "待卸货")]
    Unload = 10,

    [Display(Name = "待卸货")]
    UnSort = 20,

    [Display(Name = "待卸货")]
    d = 30,

    [Display(Name = "已分拣")]
    e = 40
}

[Display(Name = "到货通知")]
[ReceiptManagement]
public class AsnOrder : BaseOrderEntity<AsnOrderItem>
{
    [UIHint("select")]
    [KeyValue("url", "supplier/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [Display(Name = "供应商")]
    public string Supplier { get; set; } = null!;

    [Display(Name = "到货通知状态")]
    public string Status { get; set; } = null!;
}

[Display(Name = "到货通知详情")]
[ReceiptManagement]
public class AsnOrderItem : BaseOrderItemEntity<AsnOrder>
{
    [Display(Name = "商品")]
    public string Product { get; set; } = null!;

    [Display(Name = "数量")]
    public int Count { get; set; }
}
