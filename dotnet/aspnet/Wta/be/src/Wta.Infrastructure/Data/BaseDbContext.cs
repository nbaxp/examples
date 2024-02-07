using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Domain;
using Wta.Infrastructure.Extensions;
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
                //配置实体
                var modlerBuilder = modelBuilder.Entity(entityType);
                if (entityType.IsAssignableTo(typeof(BaseEntity)))
                {
                    //配置软删除、多租户的全局过滤器
                    this.GetType().GetMethod(nameof(this.CreateQueryFilter))?.MakeGenericMethod(entityType).Invoke(this, new object[] { modelBuilder });
                    //配置实体Id
                    modlerBuilder.HasKey(nameof(BaseEntity.Id));
                    modlerBuilder.Property(nameof(BaseEntity.Id)).ValueGeneratedNever();
                    //配置实体行版本号
                    if (entityType.IsAssignableTo(typeof(IConcurrencyStampEntity)))
                    {
                        modlerBuilder.Property(nameof(IConcurrencyStampEntity.ConcurrencyStamp)).ValueGeneratedNever();
                    }
                    //配置树形结构实体
                    if (entityType.IsAssignableTo(typeof(BaseTreeEntity<>).MakeGenericType(entityType)))
                    {
                        modlerBuilder.Property("Name").IsRequired();
                        modlerBuilder.Property("Number").IsRequired();
                        modlerBuilder.HasIndex("TenantId", "Number").IsUnique();
                        modlerBuilder.HasOne("Parent").WithMany("Children").HasForeignKey("ParentId").OnDelete(DeleteBehavior.SetNull);
                    }
                }
                //配置属性
                var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                properties.ForEach(prop =>
                {
                    //配置只读字段（创建后不可更新）
                    if (prop.GetCustomAttributes<ReadOnlyAttribute>().Any())
                    {
                        modlerBuilder.Property(prop.Name).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                    }
                    //值对象非空
                    if (prop.PropertyType.IsValueType && !prop.PropertyType.IsNullableType())
                    {
                        modlerBuilder.Property(prop.Name).IsRequired();
                    }
                    //配置枚举存储为字符串
                    //if (prop.PropertyType.GetUnderlyingType().IsEnum)
                    //{
                    //    modlerBuilder.Property(prop.Name).HasConversion<string>();
                    //}
                    //配置日期存取时为UTC时间
                    if (prop.PropertyType.GetUnderlyingType() == typeof(DateTime))
                    {
                        if (prop.PropertyType.IsNullableType())
                        {
                            modlerBuilder.Property<DateTime?>(prop.Name).HasConversion(v =>
                            v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime()) : null,
                            v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));
                        }
                        else
                        {
                            modlerBuilder.Property<DateTime>(prop.Name).HasConversion(v =>
                            v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                        }
                    }
                });
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
        var userName = this.GetService<IHttpContextAccessor>().HttpContext?.User.Identity?.Name ?? "admin";
        var now = DateTime.UtcNow;
        foreach (var item in entries.Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted))
        {
            //实体IsReadOnly属性为true的不可删除
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
                //第一次删除为伪删除
                if (item.State == EntityState.Deleted)
                {
                    if (!entity.IsDeleted)
                    {
                        item.State = EntityState.Modified;
                        entity.IsDeleted = true;
                    }
                }
                if (item.State == EntityState.Added)
                {
                    entity.CreatedOn = now;
                    entity.CreatedBy = userName;
                }
                else if (item.State == EntityState.Modified)
                {
                    entity.UpdatedOn = now;
                    entity.UpdatedBy = userName;
                }
                //设置行版本号
                if (item.State == EntityState.Added || item.State == EntityState.Modified)
                {
                    if (entity is IConcurrencyStampEntity concurrency)
                    {
                        concurrency.ConcurrencyStamp = Guid.NewGuid().ToString("N");
                    }
                }
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
