using Wta.Infrastructure.Domain;

namespace Wta.Application.Identity.Domain;

[IdentityGroup, Display(Order = 3)]
public class Permission : Entity
{
    /// <summary>
    /// 类型：group\menu\button(Vue Router Meta Type)
    /// </summary>
    public MenuType Type { get; set; }

    /// <summary>
    /// 权限或菜单编号(group和menu 对应 Vue Router Path，buton 对应权限标识)
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// Vue Router Redirect
    /// </summary>
    public string? Redirect { get; set; }

    /// <summary>
    /// 前端组件(Vue Router Component)
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 是否隐藏菜单或按钮
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Vue Router Meta Title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Vue Router Meta Icon
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 按钮类型：table、row
    /// </summary>
    public ButtonType ButtonType { get; set; }

    /// <summary>
    /// 按钮 html class
    /// </summary>
    public string? ButtonClass { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    public string? ApiMethod { get; set; }

    public string? Command { get; set; }

    public string? Schema { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string? ApiUrl { get; set; }

    public Guid? ParentId { get; set; }

    public Permission? Parent { get; set; }

    public List<Permission> Children { get; set; } = new List<Permission>();

    /// <summary>
    /// 角色权限
    /// </summary>
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
