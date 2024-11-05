namespace Wta.Application.Platform;

[DependsOn<PlatformModule>]
public class PlatformDbContext(DbContextOptions<PlatformDbContext> options, IServiceProvider serviceProvider) : BaseDbContext<PlatformDbContext>(options, serviceProvider)
{
}
