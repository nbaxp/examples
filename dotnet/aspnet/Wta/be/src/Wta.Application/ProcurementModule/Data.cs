using Wta.Application.SystemModule.Data;

namespace Wta.Application.ProcurementModule;

public class DbConfig : BaseDbConfig<SystemDbContext>
{
}

public class DataDbSeeder : IDbSeeder<SystemDbContext>
{
    public void Seed(SystemDbContext context)
    {
    }
}
