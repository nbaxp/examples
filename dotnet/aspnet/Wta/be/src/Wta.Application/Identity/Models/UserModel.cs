namespace Wta.Application.Identity.Models;

public class UserModel
{
    public Guid? Id { get; set; }
    public string? UserName { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Avatar { get; set; }
    public Guid? DepartmentId { get; set; }
    public List<Guid> Roles { get; set; } = new List<Guid>();
}