using Wta.Application.Platform.Data;

namespace Wta.Application.BaseData.Domain;

[DependsOn<PlatformDbContext>, UomGroup, Display(Name = "计量单位", Order = 20)]
public class Uom : BaseNameNumberEntity
{
    [Display(Name = "类型")]
    public Guid CategoryId { get; set; }

    public UnitCategory? Category { get; set; }

    [Display(Name = "比例")]
    public float Ratio { get; set; } = 1f;
}

[DependsOn<PlatformDbContext>, UomGroup, Display(Name = "计量单位分类", Order = 10)]
public class UnitCategory : BaseNameNumberEntity
{
    public List<Uom> Uoms { get; set; } = [];
}
