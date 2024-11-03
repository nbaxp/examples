using System.ComponentModel.DataAnnotations.Schema;
using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Domain;

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "资产分类", Order = 100)]
public class AssetCategory : BaseNameNumberEntity
{
    public List<Asset> Assets { get; set; } = [];
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

[Display(Name = "资产型号", Order = 105)]
[DependsOn<PlatformDbContext>]
[BaseData]
public class AssetVersion : BaseNameNumberEntity
{
    [Display(Name = "类型")]
    public AssetType Type { get; set; }

    public List<AttributeMeta> Attributes { get; set; } = [];
}

[Display(Name = "资产型号")]
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

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "资产", Order = 110)]
public class Asset : BaseTreeEntity<Asset>
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
}

//[DependsOn<PlatformDbContext>]
//[BaseData]
//[Display(Name = "扩展属性", Order = 110)]
//public class KeyValueMeta : BaseNameNumberEntity
//{
//    public string ValueType { get; set; } = default!;
//    public string InputType { get; set; } = default!;

//    [Display(Name = "单位")]
//    public string? Unit { get; set; }

//    [Display(Name = "标准单位")]
//    public string? UnitNumber { get; set; }
//}

public class KeyValue
{
    public string Key { get; set; } = default!;
    public string? Value { get; set; } = default!;
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "工位分类", Order = 120)]
public class WorkstationCategory : BaseNameNumberEntity
{
    public List<Workstation> Workstations { get; set; } = [];
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "工位", Order = 130)]
public class Workstation : BaseTreeEntity<Workstation>
{
    [UIHint("select")]
    [KeyValue("url", "workstation-category/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "分类")]
    public Guid? CategoryId { get; set; }

    public WorkstationCategory? Category { get; set; } = default!;

    [Hidden]
    public List<WorkstationDevice> WorkstationDevices { get; set; } = [];

    [UIHint("select")]
    [KeyValue("url", "asset/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [Display(Name = "设备")]
    [NotMapped]
    public List<Guid> Devices
    {
        get
        {
            return WorkstationDevices?.Select(o => o.AssetId).ToList() ?? [];
        }
        set
        {
            WorkstationDevices = value?.Distinct().Select(o => new WorkstationDevice { AssetId = o }).ToList() ?? [];
        }
    }

    [Display(Name = "班次")]
    public List<WorkstationTimeGroup> TimeGroups { get; set; } = [];
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "工位设备", Order = 140)]
public class WorkstationDevice : ISoftDelete, ITenant
{
    public Guid WorkstationId { get; set; }
    public Guid AssetId { get; set; }
    public Workstation? Workstation { get; set; }
    public Asset? Asset { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantNumber { get; set; }
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "工位设备日志", Order = 145)]
public class WorkstationDeviceLog : BaseEntity
{
    [UIHint("select")]
    [KeyValue("url", "workstation/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [Display(Name = "工位")] public string WorkstationNumber { get; set; } = default!;

    [UIHint("select")]
    [KeyValue("url", "asset/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    [KeyValue("skipSorting", true)]
    [Display(Name = "设备")]
    public string DeviceNumber { get; set; } = default!;

    public string DeviceStatusNumber { get; set; } = default!;

    [Display(Name = "开始")]
    public DateTime Start { get; set; }

    [Display(Name = "结束")]
    public DateTime End { get; set; }

    [Display(Name = "持续")]
    [ReadOnly(true)]
    [RegularExpression(@"^((\d+)\.)?(\d\d):(60|([0-5][0-9])):(60|([0-5][0-9]))$", ErrorMessage = "{0}格式错误")]
    public TimeSpan Duration { get; set; }
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "班次", Order = 150)]
public class WorkstationTimeGroup : BaseEntity
{
    [UIHint("select")]
    [KeyValue("url", "workstation/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "车间")]
    public Guid WorkstationId { get; set; }

    [Hidden]
    public Workstation? Workstation { get; set; }

    [Display(Name = "班次名称")]
    public string Name { get; set; } = default!;

    [Display(Name = "开始")]
    [UIHint("time")]
    public TimeOnly Start { get; set; }

    [Display(Name = "结束")]
    [UIHint("time")]
    public TimeOnly End { get; set; }

    public List<TimeRange> Ranges { get; set; } = [];
}

[DependsOn<PlatformDbContext>]
[BaseData]
[Display(Name = "时间段", Order = 160)]
public class TimeRange : BaseEntity
{
    [UIHint("select")]
    [KeyValue("url", "workstation-time-group/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [KeyValue("isTree", true)]
    [Display(Name = "班次")]
    public Guid GroupId { get; set; }

    public WorkstationTimeGroup? Group { get; set; }

    [Display(Name = "开始")]
    [UIHint("time")]
    public TimeOnly Start { get; set; }

    [Display(Name = "结束")]
    [UIHint("time")]
    public TimeOnly End { get; set; }
}
