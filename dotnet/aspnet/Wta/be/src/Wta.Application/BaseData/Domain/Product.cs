using Wta.Application.Platform;

namespace Wta.Application.BaseData.Domain;

[DependsOn<PlatformDbContext>]
[ProductGroup]
[Display(Name = "产品", Order = 20)]
public class Product : Entity, IEntityTypeConfiguration<Product>
{
    [UIHint("select")]
    [KeyValue("url", "product-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name ="分类")]
    public Guid CategoryId { get; set; }
    public ProductCategory? Category { get; set; }

    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Products).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}

[DependsOn<PlatformDbContext>]
[ProductGroup]
[Display(Name = "产品分类", Order = 10)]
public class ProductCategory : BaseTreeEntity<ProductCategory>
{
    [Hidden]
    public List<Product> Products { get; set; } = [];
}
