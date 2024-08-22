namespace Wta.Application.BaseData;

[Supplier, Display(Name = "供应商分类", Order = 3)]
public class SupplierCategory : BaseTreeEntity<SupplierCategory>
{
}

[Supplier, Display(Name = "供应商", Order = 4)]
public class Supplier : BaseNameNumberEntity
{
}

[Customer, Display(Name = "客户分类", Order = 5)]
public class CustomerCategory : BaseTreeEntity<CustomerCategory>
{
}

[Customer, Display(Name = "客户", Order = 6)]
public class Customer : BaseNameNumberEntity
{
}

[Product, Display(Name = "产品类型", Order = 1)]
public class ProductType : BaseNameNumberEntity
{
}

[Product, Display(Name = "产品分类", Order = 2)]
public class ProductCategory : BaseTreeEntity<ProductCategory>
{
}

[Product, Display(Name = "产品", Order = 3)]
public class Product : Entity
{
}
