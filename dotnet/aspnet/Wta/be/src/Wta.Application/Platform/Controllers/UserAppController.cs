//namespace Wta.Application.SystemModule.Controllers;

//[View("user-center/user-app")]
//public class UserAppController(IRepository<ExternalApp> repository) : BaseController, IResourceService<ExternalApp>
//{
//    [HttpGet, AllowAnonymous, Ignore]
//    public ApiResult<object> Schema()
//    {
//        return Json(typeof(ExternalApp).GetMetadataForType());
//    }

//    [HttpGet, Authorize, Ignore]
//    public ApiResult<UserInfoModel> Index()
//    {
//        //var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
//        //var result = repository
//        //   .AsNoTracking()
//        //   .Include(o => o.UserRoles)
//        //   .ThenInclude(o => o.Role)
//        //   .ThenInclude(o => o!.RolePermissions)
//        //   .ThenInclude(o => o.Permission)
//        //   .FirstOrDefault(o => o.NormalizedUserName == normalizedUserName)!;
//        //var model = mapper.ToModel<User, UserInfoModel>(result);
//        //model.Roles = result.UserRoles.Select(o => o.Role!.Number).ToList();
//        //model.Permissions = result.UserRoles.SelectMany(o => o.Role!.RolePermissions.Select(o => o.Permission!.Number)).ToList();
//        //return Json(model);
//        throw new NotImplementedException();
//    }
//}
