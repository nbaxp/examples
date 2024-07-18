namespace Wta.Application.Default.Controllers;

[View("user-center/user-info")]
public class UserInfoController(IRepository<User> repository) : BaseController, IResourceService<UserInfoModel>
{
    [Authorize, Ignore]
    public ApiResult<UserInfoModel> Index()
    {
        var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
        var result = repository
           .AsNoTracking()
           .Include(o => o.UserRoles)
           .ThenInclude(o => o.Role)
           .ThenInclude(o => o!.RolePermissions)
           .ThenInclude(o => o.Permission)
           .FirstOrDefault(o => o.NormalizedUserName == normalizedUserName)!;

        return Json(new UserInfoModel
        {
            UserName = result.UserName,
            Name = result.Name,
            Avatar = result.Avatar,
            Roles = result.UserRoles.Select(o => o.Role!.Number).ToList(),
            Permissions = result.UserRoles.SelectMany(o => o.Role!.RolePermissions).Select(o => o.Permission!.Number).ToList()
        });
    }
}
