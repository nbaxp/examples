using Wta.Infrastructure.Application.Domain;

namespace Wta.Infrastructure.Data;

public abstract class BaseDbSeeder<TContext> : IDbSeeder<TContext> where TContext : DbContext
{
    public virtual void Seed(TContext context)
    {
    }

    public virtual Guid FindIdByName<T>(TContext context, string name) where T : BaseNameNumberEntity
    {
        return context.Set<T>().First(o => o.Name == name).Id;
    }
}
