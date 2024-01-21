namespace Wta.Application.Identity.Models;

public class UserInfoModel
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public List<string?> Roles { get; set; } = new List<string?>();
    public List<string?> Permissions { get; set; } = new List<string?>();
}
