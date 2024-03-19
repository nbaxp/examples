using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Wta.Infrastructure.Application.Domain;
using Wta.Infrastructure.Tenant;

namespace Wta.Infrastructure.Data;

public abstract class BaseDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public static readonly ILoggerFactory DefaultLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    private readonly string? _tenantNumber;

    public IServiceProvider ServiceProvider { get; }
    public IDbContextManager DbContextManager { get; }
    public bool DisableSoftDeleteFilter { get; set; }
    public bool DisableTenantFilter { get; set; }

    public BaseDbContext(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        ServiceProvider = serviceProvider;
        DbContextManager = ServiceProvider.GetRequiredService<IDbContextManager>();
        DbContextManager.Add(this);
        _tenantNumber = serviceProvider.GetService<ITenantService>()?.TenantNumber;
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
        if (WtaApplication.DbContextEntities.TryGetValue(GetType(), out var entityTypes))
        {
            entityTypes.ForEach(entityType =>
            {
                //配置实体
                var modlerBuilder = modelBuilder.Entity(entityType);
                if (entityType.IsAssignableTo(typeof(BaseEntity)))
                {
                    //配置软删除、多租户的全局过滤器
                    GetType().GetMethod(nameof(this.CreateQueryFilter))?.MakeGenericMethod(entityType).Invoke(this, [modelBuilder]);
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
                        modlerBuilder.Property(nameof(BaseTreeEntity<BaseEntity>.Name)).IsRequired();
                        modlerBuilder.Property(nameof(BaseTreeEntity<BaseEntity>.Number)).IsRequired();
                        modlerBuilder.HasIndex(nameof(BaseTreeEntity<BaseEntity>.TenantNumber), nameof(BaseTreeEntity<BaseEntity>.Number)).IsUnique();
                        modlerBuilder.HasOne(nameof(BaseTreeEntity<BaseEntity>.Parent)).WithMany(nameof(BaseTreeEntity<BaseEntity>.Children)).HasForeignKey(nameof(BaseTreeEntity<BaseEntity>.ParentId)).OnDelete(DeleteBehavior.SetNull);
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
                    if (prop.PropertyType.GetUnderlyingType().IsEnum)
                    {
                        modlerBuilder.Property(prop.Name).HasConversion<string>();
                    }
                    //配置日期存取时为UTC时间
                    if (prop.PropertyType.GetUnderlyingType() == typeof(DateTime))
                    {
                        if (prop.PropertyType.IsNullableType())
                        {
                            modlerBuilder.Property<DateTime?>(prop.Name).HasConversion(v =>
                            v.HasValue ? v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime() : null,
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
                //自定义配置
                var dbConfigType = typeof(IEntityTypeConfiguration<>).MakeGenericType(entityType);
                ServiceProvider.GetServices(dbConfigType).ForEach(config =>
                {
                    var method = modelBuilder.GetType().GetMethod(nameof(modelBuilder.ApplyConfiguration))?.MakeGenericMethod(entityType);
                    method?.Invoke(modelBuilder, [config]);
                });
            });
        }
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
            //设置租户
            if (item.Entity is ITenantEntity tenantEntity)
            {
                if (item.State == EntityState.Added)
                {
                    if (_tenantNumber != null)
                    {
                        tenantEntity.TenantNumber = _tenantNumber;
                    }
                }
            }
            //设置审计属性户
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
        var eventPublisher = ServiceProvider.GetRequiredService<IEventPublisher>();
        var method = eventPublisher.GetType().GetMethod(nameof(eventPublisher.Publish))!;
        entries.ForEach(o =>
        {
            if (o.State == EntityState.Modified)
            {
                var type = typeof(EntityUpdatedEvent<>).MakeGenericType(o.Entity.GetType());
                var @event = Activator.CreateInstance(type, o.Entity);
                method.MakeGenericMethod(type).Invoke(eventPublisher, [@event]);
            }
        });
        return entries;
    }

    public void CreateQueryFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(o =>
        (DisableSoftDeleteFilter == true || !o.IsDeleted) &&
        (DisableTenantFilter == true || o.TenantNumber == _tenantNumber));
    }
}
