namespace Wta.Application.Default.Models;

[UserCenter]
[Display(Name = "用户信息", Order = 1)]
public class UserInfoModel : IResource
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public List<string> Roles { get; set; } = [];
    public List<string> Permissions { get; set; } = [];
}
