namespace Wta.Application.Default.Domain;

[Display(Name = "MES", Order = 1)]
public class MesAttribute : SystemManagementAttribute
{
}

[Display(Name = "基础数据", Order = 1)]
public class BaseDataAttribute : MesAttribute
{
}

[Display(Name = "工业设计", Order = 2)]
public class TechnologyAttribute : MesAttribute
{
}
