using Wta.Application.SystemModule.Data;

namespace Wta.Application.SystemModule.Domain;

[DependsOn<SystemDbContext>]
public class WorkGroupUser : ITenantEntity
{
    public Guid WorkGroupId { get; set; }
    public Guid UserId { get; set; }
    public string? TenantNumber { get; set; }
    public WorkGroup? WorkGroup { get; set; }
    public User? User { get; set; }
}
