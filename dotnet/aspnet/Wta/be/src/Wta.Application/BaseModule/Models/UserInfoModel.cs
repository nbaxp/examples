namespace Wta.Application.BaseModule.Models;

[UserCenter]
[Display(Name = "用户信息", Order = 1)]
[KeyValue("url", "user-info/index")]
public class UserInfoModel : IResource
{
    [ReadOnly(true)]
    [Required]
    [Display(Name = "用户名")]
    public string? UserName { get; set; }

    [Required]
    [Display(Name = "密码")]
    public string? Name { get; set; }

    [EmailAddress]
    [Display(Name = "邮箱")]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    public string? Avatar { get; set; }

    [ReadOnly(true)]
    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public Guid? DepartmentId { get; set; }

    [ReadOnly(true)]
    [UIHint("select")]
    [KeyValue("url", "role/search")]
    [KeyValue("value", "number")]
    [KeyValue("label", "name")]
    public List<string> Roles { get; set; } = [];

    [Hidden]
    public List<string> Permissions { get; set; } = [];
}
