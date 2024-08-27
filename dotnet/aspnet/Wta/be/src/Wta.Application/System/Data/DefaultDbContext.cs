namespace Wta.Application.System.Data;

[DependsOn<DefaultModule>]
public class DefaultDbContext(DbContextOptions<DefaultDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<DefaultDbContext>(options, serviceProvider)
{
}
