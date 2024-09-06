using Wta.Application.SystemModule.Data;

namespace Wta.Application.BaseDataModule;

[Supplier, Display(Name = "供应商分类", Order = 3)]
[DependsOn<SystemDbContext>]
public class SupplierCategory : BaseTreeEntity<SupplierCategory>
{
}

[Supplier, Display(Name = "供应商", Order = 4)]
[DependsOn<SystemDbContext>]
public class Supplier : BaseNameNumberEntity
{
    public Guid CategoryId { get; set; }
    [Hidden]
    public SupplierCategory? Category { get; set; }

    [Display(Name = "联系人")]
    public string Contact { get; set; } = null!;

    [Display(Name = "手机号")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "邮箱")]
    public string? Email { get; set; }
}

[Customer, Display(Name = "客户分类", Order = 5)]
[DependsOn<SystemDbContext>]
public class CustomerCategory : BaseTreeEntity<CustomerCategory>
{
}

[Customer, Display(Name = "客户", Order = 6)]
[DependsOn<SystemDbContext>]
public class Customer : BaseNameNumberEntity
{
    public Guid CategoryId { get; set; }
    [Hidden]
    public CustomerCategory? Category { get; set; }

    [Display(Name = "联系人")]
    public string Contact { get; set; } = null!;

    [Display(Name = "手机号")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "邮箱")]
    public string? Email { get; set; }
}

[Product, Display(Name = "产品类型", Order = 10)]
[DependsOn<SystemDbContext>]
public class ProductType : BaseNameNumberEntity
{
}

[Product, Display(Name = "产品分类", Order = 20)]
[DependsOn<SystemDbContext>]
public class ProductCategory : BaseTreeEntity<ProductCategory>
{
}

[Product, Display(Name = "产品单位", Order = 30)]
[DependsOn<SystemDbContext>]
public class ProductUnit : BaseNameNumberEntity
{
}

[Product, Display(Name = "产品", Order = 40)]
[DependsOn<SystemDbContext>]
public class Product : BaseNameNumberEntity
{
    [UIHint("select")]
    [KeyValue("url", "product-type/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "产品类型")]
    public Guid TypeId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "product-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "产品分类")]
    public Guid CategoryId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "product-imot/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "产品单位")]
    public Guid UnitId { get; set; }

    public ProductType? Type { get; set; }
    public ProductCategory? Category { get; set; }
    public ProductUnit? Unit { get; set; }
}
