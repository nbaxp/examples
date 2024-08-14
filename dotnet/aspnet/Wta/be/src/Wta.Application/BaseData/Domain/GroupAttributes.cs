namespace Wta.Application.BaseData.Domain;

[Display(Name = "基础数据", Order = 10)]
public class BaseDataAttribute : GroupAttribute
{
}

[Display(Name = "产品", Order = 1)]
public class ProductAttribute : BaseDataAttribute
{
}

[Display(Name = "仓库", Order = 2)]
public class WarehouseAttribute : BaseDataAttribute
{
}
