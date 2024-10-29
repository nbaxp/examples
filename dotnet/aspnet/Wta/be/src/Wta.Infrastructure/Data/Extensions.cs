namespace Wta.Infrastructure.Data;

public static class Extensions
{
    public static void AddDbContext<TDbContext>(this WebApplicationBuilder builder)
    where TDbContext : DbContext
    {
        var provider = builder.Configuration.GetValue<string>($"DbContextProviders:{typeof(TDbContext).Name}")!;
        var connectionString = builder.Configuration.GetConnectionString(typeof(TDbContext).Name)!;
        var migrationsAssemblyName = "Wta.Migrations";
        if (provider == "sqlite")
        {
            builder.Services.AddScoped<DbContext, TDbContext>();
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseSqlite(connectionString,
            b =>
            {
                b.UseNetTopologySuite();
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
    }
}
