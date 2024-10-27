using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Wta.Infrastructure.Data;

public static class Extensions
{
  public static void AddDbContext<TDbContext>(this WebApplicationBuilder builder, string connectionStringName, string assemblyName)
    where TDbContext : DbContext
  {
    var connectionStringValue = builder.Configuration.GetConnectionString(connectionStringName)!;
    var matches = Regex.Match(connectionStringValue, "(.+)://(.+)");
    var provider = matches.Groups[1].Value;
    var connectionString = matches.Groups[2].Value;
    if (provider == "sqlite")
    {
      builder.Services.AddScoped<DbContext, TDbContext>();
      builder.Services.AddDbContext<TDbContext>(
      o => o.UseSqlite(connectionString,
      b =>
      {
        b.UseNetTopologySuite();
        b.MigrationsAssembly(assemblyName);
      }));
    }
  }
}
