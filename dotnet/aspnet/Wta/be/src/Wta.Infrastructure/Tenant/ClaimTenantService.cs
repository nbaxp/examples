using Microsoft.AspNetCore.Http;

namespace Wta.Infrastructure.Tenant;

[Service<ITenantService>(ServiceLifetime.Scoped)]
public class ClaimTenantService(IServiceProvider serviceProvider) : ITenantService
{
    private string? _tenantNumber;

    public string TenantNumber
    {
        get
        {
            if (_tenantNumber != null)
            {
                return _tenantNumber;
            }
            return serviceProvider.GetService<IHttpContextAccessor>()?.HttpContext?.User.Claims.FirstOrDefault(o => o.Type == "TenantNumber")?.Value ?? TenantConstants.ROOT;
        }
        set
        {
            _tenantNumber = value;
        }
    }

    public List<string> Permissions { get; set; } = new List<string>();
}
