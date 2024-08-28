using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Wta.Application.BaseModule.Data;

public class DefaultDbContextFactory : IDesignTimeDbContextFactory<BaseDbContext>
{
    public BaseDbContext CreateDbContext(string[] args)
    {
        //WtaApplication.Run<Startup>(args);
        var optionsBuilder = new DbContextOptionsBuilder<BaseDbContext>();
        optionsBuilder.UseSqlite("Data Source=wta.db");
        return new DefaultDbContext(optionsBuilder.Options);
    }
}
