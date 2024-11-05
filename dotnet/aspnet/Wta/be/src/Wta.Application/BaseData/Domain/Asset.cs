using Wta.Application.Platform;

namespace Wta.Application.BaseData.Domain;
[DependsOn<PlatformDbContext>]
[AssetGroup]
[Display(Name = "资产", Order = 110)]
public class Asset : BaseTreeEntity<Asset>, IEntityTypeConfiguration<Asset>
{
    [UIHint("select")]
    [KeyValue("url", "asset-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "分类")]
    public Guid? CategoryId { get; set; }

    public AssetCategory? Category { get; set; }

    public List<KeyValue> Values { get; set; } = [];
    public List<WorkstationDevice> WorkstationDevices { get; set; } = [];

    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasOne(o => o.Category).WithMany(o => o.Assets).HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.SetNull);
        builder.OwnsMany(o => o.Values, o => o.ToJson());
    }
}

public enum AssetType
{
    [Display(Name = "直连设备")]
    Device = 10,

    [Display(Name = "网关")]
    Gateway = 20,

    [Display(Name = "网关子设备")]
    GatewayDevice = 30
}

[DependsOn<PlatformDbContext>]
[AssetGroup]
[Display(Name = "资产分类", Order = 100)]
public class AssetCategory : BaseNameNumberEntity
{
    public List<Asset> Assets { get; set; } = [];
}

[Display(Name = "资产型号", Order = 105)]
[DependsOn<PlatformDbContext>]
[AssetGroup]
public class AssetVersion : BaseNameNumberEntity, IEntityTypeConfiguration<AssetVersion>
{
    [Display(Name = "类型")]
    public AssetType Type { get; set; }

    public List<AttributeMeta> Attributes { get; set; } = [];

    public void Configure(EntityTypeBuilder<AssetVersion> builder)
    {
        builder.OwnsMany(o => o.Attributes, o => o.ToJson());
    }
}

[Display(Name = "扩展字段")]
public class AttributeMeta
{
    [Display(Name = "编码")]
    public string Key { get; set; } = default!;

    [Display(Name = "名称")]
    public string Name { get; set; } = default!;

    [Display(Name = "数据类型")]
    public string DataType { get; set; } = default!;

    [Display(Name = "输入类型")]
    public string InputType { get; set; } = default!;

    [Display(Name = "单位")]
    public string? Unit { get; set; }

    [Display(Name = "验证")]
    public string? Regex { get; set; }

    [Display(Name = "选项")]
    public string? Options { get; set; }
}
