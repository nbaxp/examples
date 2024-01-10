using Wta.Infrastructure.Domain;

namespace Wta.Domain.Identity;

public class Permission : Entity
{
    /// <summary>
    /// 类型：0==分组,1==菜单或按钮，2==按钮权限
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 权限或菜单名称
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 权限或菜单编号
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// 分组内首页
    /// </summary>
    public string? Redirect { get; set; }

    /// <summary>
    /// 前端组件
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 菜单图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 按钮位置
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// 按钮事件类型
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// 角色权限
    /// </summary>
    public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
