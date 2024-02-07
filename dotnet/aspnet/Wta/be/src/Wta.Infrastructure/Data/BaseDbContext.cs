using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Interfaces;

namespace Wta.Shared.Data;

public abstract class BaseDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public static readonly ILoggerFactory DefaultLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    private readonly Guid? _tenantId;

    public IServiceProvider ServiceProvider { get; }
    public IDbContextManager DbContextManager { get; }
    public bool DisableSoftDeleteFilter { get; set; }
    public bool DisableTenantFilter { get; set; }

    public BaseDbContext(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        ServiceProvider = serviceProvider;
        DbContextManager = ServiceProvider.GetRequiredService<IDbContextManager>();
        DbContextManager.Add(this);
        this._tenantId = serviceProvider.GetService<ITenantService>()?.TenantId;
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
                var entityTypeBuilder = modelBuilder.Entity(entityType);
                if (entityType.IsAssignableTo(typeof(BaseEntity)))
                {
                    //软删除、租户过滤
                    this.GetType().GetMethod(nameof(this.CreateQueryFilter))?.MakeGenericMethod(entityType).Invoke(this, new object[] { modelBuilder });
                    entityTypeBuilder.HasKey(nameof(BaseEntity.Id));
                    entityTypeBuilder.Property(nameof(BaseEntity.Id)).ValueGeneratedNever();
                    if (entityType.IsAssignableTo(typeof(IConcurrencyStampEntity)))
                    {
                        entityTypeBuilder.Property(nameof(IConcurrencyStampEntity.ConcurrencyStamp)).ValueGeneratedNever();
                    }
                    if (entityType.IsAssignableTo(typeof(BaseTreeEntity<>).MakeGenericType(entityType)))
                    {
                        entityTypeBuilder.Property("Name").IsRequired();
                        entityTypeBuilder.Property("Number").IsRequired();
                        entityTypeBuilder.HasIndex("TenantId", "Number").IsUnique();
                        entityTypeBuilder.HasOne("Parent").WithMany("Children").HasForeignKey("ParentId").OnDelete(DeleteBehavior.SetNull);
                    }
                }
                var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                //配置只读字段（不可编辑）
                properties.Where(o => o.GetCustomAttributes<ReadOnlyAttribute>().Any())
                .Select(o => o.Name)
                .ForEach(o => entityTypeBuilder.Property(o).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore));
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
        foreach (var item in entries.Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted))
        {
            if (item.State == EntityState.Deleted)
            {
                var isReadOnly = item.Entity.GetType().GetProperty("IsReadOnly")?.GetValue(item.Entity) as bool?;
                if (isReadOnly.HasValue && isReadOnly.Value)
                {
                    item.State = EntityState.Unchanged;
                }
            }
            //设置审计属性和租户
            if (item.Entity is BaseEntity entity)
            {
                //    Debug.WriteLine($"{entity.Id},{entity.GetPropertyValue<BaseEntity, string>("Number")}");
                if (item.State == EntityState.Added)
                {
                    //entity.CreatedOn = now;
                    //entity.CreatedBy = userName ?? "super";
                    //entity.IsDisabled ??= false;
                    //entity.IsReadonly ??= false;
                    entity.TenantId = _tenantId;
                }
                //    else if (item.State == EntityState.Modified)
                //    {
                //        entity.UpdatedOn = now;
                //        entity.UpdatedBy = userName;
                //    }
                //    else if (item.State == EntityState.Deleted)
                //    {
                //        if (entity is ISoftDeleteEntity)
                //        {
                //            item.State = EntityState.Modified;
                //            entity.IsDeleted = true;
                //            entity.DeletedOn = now;
                //            entity.DeletedBy = userName;
                //        }
                //        else if (entity.IsReadonly.HasValue && entity.IsReadonly.Value)
                //        {
                //            throw new Exception("内置数据无法删除");
                //        }
                //    }
                //    entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            }
        }
    }

    private List<EntityEntry> GetEntries()
    {
        ChangeTracker.DetectChanges();
        var entries = ChangeTracker.Entries().ToList();
        return entries;
    }

    public void CreateQueryFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(o =>
        (this.DisableSoftDeleteFilter == true || !o.IsDeleted) &&
        (this.DisableTenantFilter == true || o.TenantId == this._tenantId));
    }
}
