namespace Wta.Application.BaseData;

[Display(Name = "基础数据", Order = 10)]
public class BaseDataAttribute : GroupAttribute
{
}

[Display(Name = "计量单位", Order = 10)]
public class UomGroupAttribute : BaseDataAttribute
{
}

[Display(Name = "厂区信息", Order = 20)]
public class WorkstationGroupAttribute : BaseDataAttribute
{
}

[Display(Name = "资产信息", Order = 30)]
public class AssetGroupAttribute : BaseDataAttribute
{
}

[Display(Name = "产品信息", Order = 40)]
public class ProductGroupAttribute : BaseDataAttribute
{
}
