namespace Wta.Infrastructure.Interfaces;

public interface IAuthService
{
    bool HasPermission(string permission);
}
