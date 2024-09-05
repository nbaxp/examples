namespace Wta.Application.SystemModule.Data;

[DependsOn<Module>]
public class SystemDbContext(DbContextOptions<SystemDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<SystemDbContext>(options, serviceProvider)
{
}
