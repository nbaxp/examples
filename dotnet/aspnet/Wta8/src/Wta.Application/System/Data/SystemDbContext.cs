using Microsoft.EntityFrameworkCore;
using Wta.Infrastructure.Data;

namespace Wta.Application.System.Data;

public class SystemDbContext(DbContextOptions<SystemDbContext> options) : BaseDbContext<SystemDbContext>(options)
{
}