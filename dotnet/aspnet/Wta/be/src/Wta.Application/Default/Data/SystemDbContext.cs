using Wta.Shared.Data;

namespace Wta.Application.Default.Data;

public class SystemDbContext(DbContextOptions<SystemDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<SystemDbContext>(options, serviceProvider)
{
}
