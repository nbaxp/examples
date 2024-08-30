using Wta.Application.BaseModule.Data;

namespace Wta.Application.ProcurementModule;

public class DbConfig : BaseDbConfig<BaseDbContext>
{
}

public class DataDbSeeder : IDbSeeder<BaseDbContext>
{
    public void Seed(BaseDbContext context)
    {
    }
}
