namespace Wta.Application.BaseModule.Data;

[DependsOn<DefaultModule>]
public class BaseDbContext(DbContextOptions<BaseDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<BaseDbContext>(options, serviceProvider)
{
}
