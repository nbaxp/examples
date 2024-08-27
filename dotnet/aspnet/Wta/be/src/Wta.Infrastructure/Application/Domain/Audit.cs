namespace Wta.Infrastructure.Application.Domain;
public class Audit
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public long EntityVersion { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = default!;
}
