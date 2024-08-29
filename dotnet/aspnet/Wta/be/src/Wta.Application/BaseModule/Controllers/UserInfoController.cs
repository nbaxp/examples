using Wta.Infrastructure.Mapper;

namespace Wta.Application.BaseModule.Controllers;

[View("user-center/user-info")]
public class UserInfoController(IObjerctMapper mapper, IRepository<User> repository) : BaseController, IResourceService<UserInfoModel>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Schema()
    {
        return Json(typeof(UserInfoModel).GetMetadataForType());
    }

    [HttpGet, Authorize, Ignore]
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
        var model = mapper.ToModel<User, UserInfoModel>(result);
        model.Roles = result.UserRoles.Select(o => o.Role!.Number).ToList();
        model.Permissions = result.UserRoles.SelectMany(o => o.Role!.RolePermissions.Select(o => o.Permission!.Number)).ToList();
        return Json(model);
    }

    [HttpPost, Authorize, Ignore]
    public ApiResult<bool> Index(UserInfoModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        if (model.UserName != User.Identity?.Name)
        {
            ModelState.AddModelError(nameof(model.UserName), "Error");
            throw new BadRequestException();
        }
        var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
        var user = repository.Query().FirstOrDefault(o => o.NormalizedUserName == normalizedUserName)!;
        mapper.FromModel(user, model);
        repository.SaveChanges();
        return Json(true);
    }

    //[HttpGet,Authorize]
    //public ApiResult<UserInfoModel> Index()
    //{
    //    var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
    //    var result = repository
    //       .AsNoTracking()
    //       .Include(o => o.UserRoles)
    //       .ThenInclude(o => o.Role)
    //       .ThenInclude(o => o!.RolePermissions)
    //       .ThenInclude(o => o.Permission)
    //       .FirstOrDefault(o => o.NormalizedUserName == normalizedUserName)!;

    //    return Json(new UserInfoModel
    //    {
    //        UserName = result.UserName,
    //        Name = result.Name,
    //        Avatar = result.Avatar,
    //        Roles = result.UserRoles.Select(o => o.Role!.Number).ToList(),
    //        Permissions = result.UserRoles.SelectMany(o => o.Role!.RolePermissions).Select(o => o.Permission!.Number).ToList()
    //    });
    //}
}
