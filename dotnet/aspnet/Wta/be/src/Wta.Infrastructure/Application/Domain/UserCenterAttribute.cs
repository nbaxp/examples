namespace Wta.Infrastructure.Application.Domain;

[Display(Name = "用户中心", Order = -10), KeyValue("Redirect", "/user-center/user-info")]
public class UserCenterAttribute : GroupAttribute
{
}
