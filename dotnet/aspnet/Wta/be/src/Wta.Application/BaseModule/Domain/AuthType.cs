namespace Wta.Infrastructure.Application.Domain;
public enum AuthType
{
    [Display(Name = "匿名")]
    Anonymous,
    [Display(Name = "已登录")]
    Authorize,
    [Display(Name = "角色")]
    Roles,
    [Display(Name = "指定权限")]
    Permission,
    [Display(Name = "自定义")]
    Custom
}
