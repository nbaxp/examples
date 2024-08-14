namespace Wta.Application.Wms.Domain;

[BaseData, Display(Name = "入库单", Order = 1)]
public class StorageIn : Entity
{
}

[BaseData, Display(Name = "出库单", Order = 2)]
public class StorageOut : Entity
{
}
