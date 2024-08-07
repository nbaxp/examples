namespace Wta.Application.Default.Models;

[Display(Name = "用户")]
public class UserModel
{
    [Hidden]
    public Guid? Id { get; set; }

    [Display(Name = "登录名")]
    public string? UserName { get; set; }

    [Display(Name = "用户名")]
    public string? Name { get; set; }

    [DataType(DataType.Password)]
    [KeyValue("placeholder", "保持为空则不更新")]
    [Display(Name = "密码")]
    public string? Password { get; set; }

    [EmailAddress]
    [Display(Name = "邮箱")]
    public string? Email { get; set; }

    [Phone]
    [Display(Name = "手机号")]
    public string? PhoneNumber { get; set; }

    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    [Display(Name = "头像")]
    public string? Avatar { get; set; }

    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "部门")]
    public Guid? DepartmentId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "post/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    [Display(Name = "岗位")]
    public Guid? PostId { get; set; }

    [UIHint("select")]
    [KeyValue("url", "role/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public List<Guid> Roles { get; set; } = [];
}
