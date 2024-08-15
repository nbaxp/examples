namespace Wta.Application.BaseData.Domain;

[BaseDataGroup, Display(Name = "供应商", Order = 3)]
public class Supplier : Entity
{
}

[BaseDataGroup, Display(Name = "客户", Order = 4)]
public class Customer : Entity
{
}

[Product, Display(Name = "产品类型", Order = 1)]
public class ProductType : BaseTreeEntity<ProductType>
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

[Warehouse, Display(Name = "仓库类型", Order = 1)]
public class WarehouseType : BaseTreeEntity<WarehouseType>
{
}

[Warehouse, Display(Name = "仓库", Order = 2)]
public class Warehouse : BaseTreeEntity<Warehouse>
{
    [Display(Name = "地址")]
    public string? Address { get; set; } = null!;

    [Display(Name = "部门")]
    public string? Department { get; set; } = null!;

    [Display(Name = "负责人")]
    public string? User { get; set; } = null!;

    [Display(Name = "联系方式")]
    public string? Contact { get; set; } = null!;
}

[Warehouse, Display(Name = "仓位", Order = 3)]
public class StorageArea : BaseTreeEntity<StorageArea>
{
    [Display(Name = "容量")]
    public int Capacity { get; set; }

    [Display(Name = "部门")]
    public string? Department { get; set; } = null!;

    [Display(Name = "负责人")]
    public string? User { get; set; } = null!;

    [Display(Name = "联系方式")]
    public string? Contact { get; set; } = null!;
}
