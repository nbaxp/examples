namespace Wta.Application.Default.Data;

[DependsOn<DefaultModule>]
public class DefaultDbContext(DbContextOptions<DefaultDbContext> options) : BaseDbContext<DefaultDbContext>(options)
{
}
