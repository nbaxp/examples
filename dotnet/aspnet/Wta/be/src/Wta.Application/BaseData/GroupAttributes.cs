namespace Wta.Application.BaseData;

[Display(Name = "基础数据", Order = 10)]
public class BaseDataGroupAttribute : GroupAttribute
{
}

[Display(Name = "产品", Order = 1)]
public class ProductAttribute : BaseDataGroupAttribute
{
}

[Display(Name = "仓库", Order = 2)]
public class WarehouseAttribute : BaseDataGroupAttribute
{
}
