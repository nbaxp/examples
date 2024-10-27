using Microsoft.EntityFrameworkCore;

namespace Wta.Infrastructure.Data;

public abstract class BaseDbContext<TDbContext>(DbContextOptions<TDbContext> options) : DbContext(options)
  where TDbContext : DbContext
{
}
