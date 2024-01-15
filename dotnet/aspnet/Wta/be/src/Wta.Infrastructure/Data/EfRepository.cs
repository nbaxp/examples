using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Interfaces;
using Wta.Shared;

namespace Wta.Infrastructure.Data;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    public EfRepository(IServiceProvider serviceProvider)
    {
        var dbContextType = WebApp.Instance.EntityDbContextDictionary[typeof(TEntity)];
        Context = (serviceProvider.GetRequiredService(dbContextType) as DbContext)!;
        DbSet = Context.Set<TEntity>();
    }

    protected DbContext Context { get; }
    protected DbSet<TEntity> DbSet { get; }

    public void Add(TEntity entity)
    {
        DbSet.Add(entity);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        DbSet.AddRange(entities);
    }

    public IQueryable<TEntity> AsNoTracking()
    {
        return DbSet.AsNoTracking();
    }

    public void BeginTransaction()
    {
        Context.Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Context.Database.CommitTransaction();
    }

    public IQueryable<TEntity> Query()
    {
        return DbSet;
    }

    public void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }
}
