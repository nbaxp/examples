using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wta.Web.IoTGateway.Domain;

public class Driver : BaseEntity, IEntityTypeConfiguration<Driver>
{
    public string Name { get; set; } = default!;
    public string Value { get; set; } = default!;

    public void Configure(EntityTypeBuilder<Driver> builder)
    {
    }
}
