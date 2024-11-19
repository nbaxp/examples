namespace Wta.Infrastructure.Data;

public static class Extensions
{
    public static void AddDbContext<TDbContext>(this WebApplicationBuilder builder)
    where TDbContext : DbContext
    {
        var connectionStringValues = builder.Configuration.GetConnectionString(typeof(TDbContext).Name)!.Split(":");
        var provider = connectionStringValues[0];
        var connectionString = connectionStringValues[1];
        var migrationsAssemblyName = "Wta.Migrations";
        builder.Services.AddScoped<DbContext, TDbContext>();
        var useMigration = builder.Configuration.GetValue("App:UseMigration", false);
        if (provider == "mysql")
        {
            //Oracle Provider
            //builder.Services.AddDbContext<TDbContext>(
            //o => o.UseMySQL(connectionString,
            //b =>
            //{
            //    b.MigrationsAssembly(migrationsAssemblyName);
            //}));

            builder.Services.AddDbContext<TDbContext>(
            o => o.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
            b =>
            {
                b.UseMicrosoftJson(MySqlCommonJsonChangeTrackingOptions.FullHierarchyOptimizedSemantically);
                b.UseNetTopologySuite();
                if (useMigration)
                {
                    b.MigrationsAssembly(migrationsAssemblyName);
                }
            }));
        }
        else if (provider == "sqlite")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseSqlite(connectionString,
            b =>
            {
                b.UseNetTopologySuite();
                if (useMigration)
                {
                    b.MigrationsAssembly(migrationsAssemblyName);
                }
            }));
        }
        else if (provider == "npgsql")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseNpgsql(connectionString, b =>
            {
                if (useMigration)
                {
                    b.MigrationsAssembly(migrationsAssemblyName);
                }
            }));
        }
        else if (provider == "sqlserver")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseSqlServer(connectionString, b =>
            {
                if (useMigration)
                {
                    b.MigrationsAssembly(migrationsAssemblyName);
                }
            }));
        }
        else if (provider == "oracle")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseOracle(connectionString, b =>
            {
                if (useMigration)
                {
                    b.MigrationsAssembly(migrationsAssemblyName);
                }
            }));
        }
        else
        {
            throw new Exception($"数据提供程序{provider}不存在");
        }
    }
}
