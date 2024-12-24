using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wta.Web.IoTGateway.Domain;

public class User : BaseEntity, IEntityTypeConfiguration<User>
{
    public string UserName { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Salt { get; set; } = default!;

    public void Configure(EntityTypeBuilder<User> builder)
    {
    }
}
