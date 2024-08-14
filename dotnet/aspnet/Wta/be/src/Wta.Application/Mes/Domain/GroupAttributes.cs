namespace Wta.Application.System.Domain;

[Display(Name = "MES", Order = 2)]
public class MesAttribute : GroupAttribute
{
}

[Display(Name = "基础数据", Order = 1)]
public class BaseDataAttribute : MesAttribute
{
}

[Display(Name = "工艺设计", Order = 2)]
public class TechnologyAttribute : MesAttribute
{
}
