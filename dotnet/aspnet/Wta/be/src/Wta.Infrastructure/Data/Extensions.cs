using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wta.Infrastructure.Data;

public static class Extensions
{
    public static void ToJsonExt<TEntity, TRelatedEntity, TProperty>(this EntityTypeBuilder<TEntity> builder, Expression<Func<TEntity, IEnumerable<TRelatedEntity>?>> navigationExpression, Expression<Func<TEntity, IEnumerable<TProperty>>> propertyExpression)
        where TEntity : class
        where TRelatedEntity : class
    {
        builder.Property(propertyExpression).HasColumnType("json");
        builder.OwnsMany(navigationExpression, o => o.ToJson());
    }

    public static void AddDbContext<TDbContext>(this WebApplicationBuilder builder)
    where TDbContext : DbContext
    {
        var provider = builder.Configuration.GetValue<string>($"DbContextProviders:{typeof(TDbContext).Name}")?.ToLowerInvariant()!;
        var connectionString = builder.Configuration.GetConnectionString(typeof(TDbContext).Name)!;
        var migrationsAssemblyName = "Wta.Migrations";
        builder.Services.AddScoped<DbContext, TDbContext>();
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
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
        else if (provider == "sqlite")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseSqlite(connectionString,
            b =>
            {
                b.UseNetTopologySuite();
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
        else if (provider == "npgsql")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseNpgsql(connectionString, b =>
            {
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
        else if (provider == "sqlserver")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseSqlServer(connectionString, b =>
            {
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
        else if (provider == "oracle")
        {
            builder.Services.AddDbContext<TDbContext>(
            o => o.UseOracle(connectionString, b =>
            {
                b.MigrationsAssembly(migrationsAssemblyName);
            }));
        }
        else
        {
            throw new Exception($"数据提供程序{provider}不存在");
        }
    }
}
