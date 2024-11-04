using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wta.Infrastructure.Application.Domain;

public class Audit:ITenant, IEntityTypeConfiguration<Audit>
{
    public Guid Id { get; set; }
    public Guid EntityId { get; set; }
    public string EntityName { get; set; } = null!;
    public string Action { get; set; } = null!;
    public long EntityVersion { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string? TenantNumber { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.EntityName);
        builder.HasIndex(o => o.EntityId);
    }
}
