namespace Wta.Application.Default.Models;

public class UserModel
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }

    [DataType(DataType.Password)]
    [KeyValue("placeholder", "保持为空则不更新")]
    public string? Password { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Avatar { get; set; }

    [UIHint("select")]
    [KeyValue("url", "department/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public Guid? DepartmentId { get; set; }
    [UIHint("select")]
    [KeyValue("url", "role/search")]
    [KeyValue("value", "id")]
    [KeyValue("label", "name")]
    public List<Guid> Roles { get; set; } = [];
}
