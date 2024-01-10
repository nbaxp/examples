namespace Wta.Application.Identity.Models;

public class UserModel
{
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Avatar { get; set; }
    public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
}
