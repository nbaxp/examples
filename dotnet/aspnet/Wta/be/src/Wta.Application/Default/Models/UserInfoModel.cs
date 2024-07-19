namespace Wta.Application.Default.Models;

[UserCenter]
[Display(Name = "用户信息", Order = 1)]
public class UserInfoModel : IResource
{
    [ReadOnly(true)]
    public string? UserName { get; set; }
    public string? Name { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [UIHint("image")]
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
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public List<Guid> Roles { get; set; } = [];
}
