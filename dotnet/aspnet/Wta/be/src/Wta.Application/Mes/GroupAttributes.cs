namespace Wta.Application.Mes;

[Display(Name = "MES", Order = 30)]
public class MesGroupAttribute : GroupAttribute
{
}

[Display(Name = "基础数据", Order = 1)]
public class BaseDataAttribute : MesGroupAttribute
{
}

[Display(Name = "工艺设计", Order = 2)]
public class TechnologyAttribute : MesGroupAttribute
{
}
