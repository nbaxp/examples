using Wta.Shared.Data;

namespace Wta.Application.Identity.Data;

public class IdentityDbContext : BaseDbContext<IdentityDbContext>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, IServiceProvider serviceProvider) : base(options, serviceProvider)
    {
    }
}
