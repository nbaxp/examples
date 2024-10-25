using Microsoft.AspNetCore.Builder;

namespace Wta.Infrastructure.Module;

public interface IApplicationModule
{
    void ConfigureModule(WebApplication app);

    void ConfigureModuleServices(WebApplicationBuilder builder);

    //void OnConfiguring(DbContextOptionsBuilder optionsBuilder);

    //void OnModelCreating(ModelBuilder modelBuilder);

    //void Seed(TDbContext context);
}