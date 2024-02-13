using Microsoft.AspNetCore.Http;
using Wta.Infrastructure.Interfaces;

namespace Wta.Infrastructure.Services;

//[Service<ITenantService>(ServiceLifetime.Scoped)]
public class DefaultTenantService : ITenantService
{
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private Guid? _tenantId;

    public DefaultTenantService(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId
    {
        get
        {
            if (_tenantId.HasValue)
            {
                return _tenantId;
            }
            var tenantIdValue = _httpContextAccessor!.HttpContext?.User.Claims.FirstOrDefault(o => o.Type == "TenantId")?.Value;
            Guid? tenantId = string.IsNullOrEmpty(tenantIdValue) ? null : Guid.Parse(tenantIdValue);
            return tenantId;
        }
        set
        {
            _tenantId = value;
        }
    }

    public List<string> Permissions { get; set; } = new List<string>();
}
