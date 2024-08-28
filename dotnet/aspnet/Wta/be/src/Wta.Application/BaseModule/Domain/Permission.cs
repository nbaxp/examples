namespace Wta.Application.BaseModule.Domain;

/// <summary>
/// name=>meta.title,number=>meta.path
/// </summary>
[PermissionManagement, Display(Name = "权限", Order = 5)]
public class Permission : BaseTreeEntity<Permission>
{
    /// <summary>
    /// Vue Router Meta
    /// "Anonymous"
    /// "Authenticated"
    /// Roles:"['role1','role2']"
    /// "Permission"
    /// </summary>
    [Display(Name = "验证方式")]
    public string Authorize { get; set; } = default!;

    /// <summary>
    /// Vue Router Meta,按钮类型：table、row
    /// </summary>
    [Display(Name = "按钮类型")]
    public ButtonType? ButtonType { get; set; }

    /// <summary>
    /// Vue Router Meta,按钮 html class
    /// </summary>
    [Display(Name = "HTML Class")]
    public string? ClassList { get; set; }

    /// <summary>
    /// Vue Router Meta
    /// </summary>
    [Display(Name = "命令")]
    public string? Command { get; set; }

    /// <summary>
    /// Vue Router
    /// </summary>
    [Display(Name = "前端路由组件")]
    public string? Component { get; set; }

    /// <summary>
    /// 禁用，不可赋予角色
    /// </summary>
    [Display(Name = "禁用")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Vue Router Meta,是否隐藏菜单或按钮
    /// </summary>
    [Display(Name = "隐藏")]
    public bool Hidden { get; set; }

    /// <summary>
    /// Vue Router Meta
    /// </summary>
    [Display(Name = "图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// Vue Router Meta,请求方法
    /// </summary>
    [Display(Name = "HTTP Method")]
    public string? Method { get; set; }

    /// <summary>
    /// Vue Router Meta
    /// </summary>
    [Display(Name = "禁止缓存")]
    public bool NoCache { get; set; }

    /// <summary>
    /// Vue Router
    /// </summary>
    [Display(Name = "前端路由跳转")]
    public string? Redirect { get; set; }

    /// <summary>
    /// 角色权限
    /// </summary>
    [Hidden]
    public List<RolePermission> RolePermissions { get; set; } = [];

    [Display(Name = "前端路由Path")]
    public string RoutePath { get; set; } = default!;

    ///// <summary>
    ///// Vue Router Meta
    ///// </summary>
    //[Display(Name = "前端路由跳转")]
    //public string? Schema { get; set; }

    /// <summary>
    /// Vue Router Meta,group\menu\button)
    /// </summary>
    [Display(Name = "类型")]
    public MenuType Type { get; set; }
    /// <summary>
    /// Vue Router Meta,请求地址
    /// </summary>
    [Display(Name = "后端API地址")]
    public string? Url { get; set; }
}
