namespace Wta.Application.BaseData;

[Display(Name = "基础数据", Order = 10)]
public class BaseAttribute : GroupAttribute
{
}

[Display(Name = "供应商", Order = 2)]
public class SupplierAttribute : BaseAttribute
{
}

[Display(Name = "客户", Order = 3)]
public class CustomerAttribute : BaseAttribute
{
}

[Display(Name = "产品", Order = 1)]
public class ProductAttribute : BaseAttribute
{
}
