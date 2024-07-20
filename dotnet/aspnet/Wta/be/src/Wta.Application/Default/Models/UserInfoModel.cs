namespace Wta.Application.Default.Models;

[UserCenter]
[Display(Name = "用户信息", Order = 1)]
[KeyValue("url", "user-info/index")]
public class UserInfoModel : IResource
{
    [ReadOnly(true)]
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [UIHint("image-upload")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    public string? Avatar { get; set; }

    [UIHint("image-inline")]
    [KeyValue("accept", ".svg,.png")]
    [KeyValue("url", "file/upload")]
    [Required]
    public string? Avatar2 { get; set; }

    [ReadOnly(true)]
    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public Guid? DepartmentId { get; set; }

    [ReadOnly(true)]
    [UIHint("select")]
    [KeyValue("url", "role/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public List<Guid> Roles { get; set; } = [];
}
