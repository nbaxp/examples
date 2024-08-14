namespace Wta.Application.System.Services;

public class AuthService(IRepository<User> repository, IHttpContextAccessor httpContextAccessor, ILogger<AuthService> logger) : IAuthService
{
    [Authorize, Ignore]
    public bool HasPermission(string permission)
    {
        var normalizedUserName = httpContextAccessor.HttpContext?.User.Identity?.Name?.ToUpperInvariant()!;
        logger.LogInformation($"Has Permission:{permission}");
        return repository.AsNoTracking()
            .Any(o => o.NormalizedUserName == normalizedUserName && o.UserRoles.Any(o => o.Role!.RolePermissions.Any(o => o.Permission!.Type == MenuType.Button && o.Permission!.Number == permission)));
    }
}
