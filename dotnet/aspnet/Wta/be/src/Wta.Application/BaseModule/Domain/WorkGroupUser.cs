namespace Wta.Application.BaseModule.Domain;

public class WorkGroupUser
{
    public Guid WorkGroupId { get; set; }
    public Guid UserId { get; set; }
    public WorkGroup? WorkGroup { get; set; }
    public User? User { get; set; }
}
