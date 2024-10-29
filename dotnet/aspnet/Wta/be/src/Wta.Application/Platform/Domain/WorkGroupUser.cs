using Wta.Application.Platform.Data;

namespace Wta.Application.Platform.Domain;

[DependsOn<PlatformDbContext>]
public class WorkGroupUser : ITenant
{
    public Guid WorkGroupId { get; set; }
    public Guid UserId { get; set; }
    public string? TenantNumber { get; set; }
    public WorkGroup? WorkGroup { get; set; }
    public User? User { get; set; }
}
