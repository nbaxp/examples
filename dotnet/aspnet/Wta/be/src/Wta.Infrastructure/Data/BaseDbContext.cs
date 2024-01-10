using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Extensions;

namespace Wta.Shared.Data;

public abstract class BaseDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public static readonly ILoggerFactory DefaultLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    public string? _tenantId;

    public IServiceProvider ServiceProvider { get; }
    public IDbContextManager DbContextManager { get; }

    public BaseDbContext(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        ServiceProvider = serviceProvider;
        DbContextManager = ServiceProvider.GetRequiredService<IDbContextManager>();
        DbContextManager.Add(this);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(DefaultLoggerFactory);
        if (ServiceProvider.GetRequiredService<IHostEnvironment>().IsDevelopment())
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //通用配置
        if (WebApp.Instance.DbContextEntityDictionary.TryGetValue(GetType(), out var entityTypes))
        {
            entityTypes.ForEach(entityType =>
            {
                if (entityType.IsAssignableTo(typeof(BaseEntity)))
                {
                    var entityTypeBuilder = modelBuilder.Entity(entityType);
                    entityTypeBuilder.HasKey(nameof(Entity.Id));
                    entityTypeBuilder.Property(nameof(Entity.Id)).ValueGeneratedNever();
                    if (entityType.IsAssignableTo(typeof(IConcurrencyStampEntity)))
                    {
                        entityTypeBuilder.Property(nameof(IConcurrencyStampEntity.ConcurrencyStamp)).ValueGeneratedNever();
                    }
                    if (entityType.IsAssignableTo(typeof(BaseTreeEntity<>).MakeGenericType(entityType)))
                    {
                        entityTypeBuilder.HasOne("Parent").WithMany("Children").HasForeignKey("ParentId").OnDelete(DeleteBehavior.SetNull);
                    }
                }
            });
        }
        //自定义配置
        var dbConfigType = typeof(IDbConfig<>).MakeGenericType(typeof(TDbContext));
        ServiceProvider.GetServices(dbConfigType).ForEach(config =>
        {
            config!.GetType().GetInterfaces()
                .Where(type => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .ForEach(item =>
                {
                    var entityType = item.GetGenericArguments()[0];
                    var method = modelBuilder.GetType().GetMethod(nameof(modelBuilder.ApplyConfiguration))?.MakeGenericMethod(entityType);
                    method?.Invoke(modelBuilder, [config]);
                });
        });
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        var entries = GetEntries();
        BeforeSave(entries);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var entries = GetEntries();
        BeforeSave(entries);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected virtual void BeforeSave(List<EntityEntry> entries)
    {
        foreach (var item in entries.Where(o => o.State == EntityState.Deleted))
        {
        }
        //var userName = this.GetService<IHttpContextAccessor>().HttpContext?.User.Identity?.Name;
        //var tenant = this.GetService<ITenantService>().TenantId;
        //var now = DateTime.UtcNow;
        //foreach (var item in entries.Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted))
        //{
        //    // 设置审计属性和租户
        //    if (item.Entity is BaseEntity entity)
        //    {
        //        Debug.WriteLine($"{entity.Id},{entity.GetPropertyValue<BaseEntity, string>("Number")}");
        //        if (item.State == EntityState.Added)
        //        {
        //            entity.CreatedOn = now;
        //            entity.CreatedBy = userName ?? "super";
        //            entity.TenantId = tenant;
        //            entity.IsDisabled ??= false;
        //            entity.IsReadonly ??= false;
        //        }
        //        else if (item.State == EntityState.Modified)
        //        {
        //            entity.UpdatedOn = now;
        //            entity.UpdatedBy = userName;
        //        }
        //        else if (item.State == EntityState.Deleted)
        //        {
        //            if (entity is ISoftDeleteEntity)
        //            {
        //                item.State = EntityState.Modified;
        //                entity.IsDeleted = true;
        //                entity.DeletedOn = now;
        //                entity.DeletedBy = userName;
        //            }
        //            else if (entity.IsReadonly.HasValue && entity.IsReadonly.Value)
        //            {
        //                throw new Exception("内置数据无法删除");
        //            }
        //        }
        //        entity.ConcurrencyStamp = Guid.NewGuid().ToString();
        //    }
        //}
    }

    private List<EntityEntry> GetEntries()
    {
        ChangeTracker.DetectChanges();
        var entries = ChangeTracker.Entries().ToList();
        return entries;
    }
}
