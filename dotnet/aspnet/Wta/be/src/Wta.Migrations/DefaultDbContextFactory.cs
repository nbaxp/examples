using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wta.Application.SystemModule.Data;

public class DefaultDbContextFactory : IDesignTimeDbContextFactory<SystemDbContext>
{
    public SystemDbContext CreateDbContext(string[] args)
    {
        //WtaApplication.Run<Startup>(args);
        var optionsBuilder = new DbContextOptionsBuilder<SystemDbContext>();
        optionsBuilder.UseSqlite("Data Source=wta.db");
        return new DefaultDbContext(optionsBuilder.Options);
    }
}
