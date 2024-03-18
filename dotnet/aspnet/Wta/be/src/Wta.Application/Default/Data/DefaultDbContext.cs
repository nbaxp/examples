using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Data;

namespace Wta.Application.Default.Data;

[DependsOn<DefaultModule>]
public class DefaultDbContext(DbContextOptions<DefaultDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<DefaultDbContext>(options, serviceProvider)
{
}
