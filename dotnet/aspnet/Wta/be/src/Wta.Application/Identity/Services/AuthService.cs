using Microsoft.AspNetCore.Http;
using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Interfaces;

namespace Wta.Application.Identity.Services;

[Service<IAuthService>]
public class AuthService(IRepository<User> repository, IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public bool HasPermission(string permission)
    {
        var userName = httpContextAccessor.HttpContext!.User.Identity!.Name;
        return repository.AsNoTracking()
            .Any(o => o.UserName == userName && o.UserRoles.Any(o => o.Role!.RolePermissions.Any(o => o.Permission!.Type == MenuType.Button && o.Permission!.Path == permission)));
    }
}
