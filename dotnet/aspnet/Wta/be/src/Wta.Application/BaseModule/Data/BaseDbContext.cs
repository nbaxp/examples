namespace Wta.Application.BaseModule.Data;

[DependsOn<Module>]
public class BaseDbContext(DbContextOptions<BaseDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<BaseDbContext>(options, serviceProvider)
{
}
