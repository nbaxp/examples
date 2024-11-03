namespace Wta.Application.BaseData;

[Display(Name = "基础数据", Order = 10)]
public class BaseDataAttribute : GroupAttribute
{
}

[Display(Name = "计量单位", Order = 10)]
public class UomGroupAttribute : BaseDataAttribute
{
}
