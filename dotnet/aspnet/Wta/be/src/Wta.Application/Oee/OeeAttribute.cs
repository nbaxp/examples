namespace Wta.Application.Oee;

[Display(Name = "OEE", Order = 14)]
public class OeeAttribute : GroupAttribute
{
}

[Display(Name = "OEE设置", Order = 10)]
public class OeeSettingsAttribute : OeeAttribute
{
}

[Display(Name = "OEE基础数据", Order = 40)]
public class OeeBaseDataAttribute : OeeSettingsAttribute
{
}
