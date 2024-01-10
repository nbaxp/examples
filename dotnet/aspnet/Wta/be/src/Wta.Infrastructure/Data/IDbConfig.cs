using Microsoft.EntityFrameworkCore;

namespace Wta.Shared.Data;

public interface IDbConfig<TDbContext> where TDbContext : DbContext
{
}
