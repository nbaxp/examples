namespace Wta.Application.BaseDataModule;

[Display(Name = "基础数据", Order = 10)]
public class BaseDataAttribute : GroupAttribute
{
}

[Display(Name = "供应商", Order = 2)]
public class SupplierAttribute : BaseDataAttribute
{
}

[Display(Name = "客户", Order = 3)]
public class CustomerAttribute : BaseDataAttribute
{
}

[Display(Name = "产品", Order = 1)]
public class ProductAttribute : BaseDataAttribute
{
}
