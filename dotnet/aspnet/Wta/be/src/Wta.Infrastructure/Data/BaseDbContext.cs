using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Wta.Infrastructure.Application.Domain;
using Wta.Infrastructure.Tenant;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Wta.Infrastructure.Data;

public abstract class BaseDbContext<TDbContext> : DbContext where TDbContext : DbContext
{
    public static readonly ILoggerFactory DefaultLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

    private readonly string _tenantNumber;

    public BaseDbContext(DbContextOptions<TDbContext> options, IServiceProvider serviceProvider) : base(options)
    {
        ServiceProvider = serviceProvider;
        ServiceProvider.GetService<IDbContextManager>()?.Add(this);
        _tenantNumber = ServiceProvider.GetRequiredService<ITenantService>().TenantNumber;
    }

    public bool DisableSoftDeleteFilter { get; set; }
    public bool DisableTenantFilter { get; set; }
    public IServiceProvider ServiceProvider { get; }

    public void CreateQueryFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(o =>
        (DisableSoftDeleteFilter == true || !o.IsDeleted) &&
        (DisableTenantFilter == true || o.TenantNumber == _tenantNumber));
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        BeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        BeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected virtual void BeforeSave()
    {
        //本地事件
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
        //更新属性
        var userName = this.GetService<IHttpContextAccessor>().HttpContext?.User.Identity?.Name ?? "admin";
        var now = DateTime.UtcNow;
        var list = new List<string>();
        foreach (var item in entries.Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted))
        {
            if (item.Entity is Audit audit)
            {
                continue;
            }
            //设置租户
            if (item.Entity is ITenant tenantEntity && item.State == EntityState.Added)
            {
                if (_tenantNumber != null)
                {
                    tenantEntity.TenantNumber = _tenantNumber;
                }
            }
            else
            {
            }
            //实体IsReadOnly属性为true的不可删除
            if (item.State == EntityState.Deleted)
            {
                var isReadOnly = item.Entity.GetType().GetProperty("IsReadOnly")?.GetValue(item.Entity) as bool?;
                if (isReadOnly.HasValue && isReadOnly.Value)
                {
                    item.State = EntityState.Unchanged;
                }
            }
            //设置审计属性户
            if (item.Entity is BaseEntity entity)
            {
                //逻辑删除的才可以物理删除
                if (item.State == EntityState.Deleted)
                {
                    if (!entity.IsDeleted)
                    {
                        item.State = EntityState.Unchanged;
                    }
                }
                if (item.State == EntityState.Added)
                {
                    entity.CreatedOn = now;
                    entity.CreatedBy = userName;
                    entity.UpdatedOn = null;
                    entity.UpdatedBy = null;
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
        //添加实体历史记录
        var jsonSerializerOptions = ServiceProvider.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        foreach (var item in entries.Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted))
        {
            if (item.Entity is not Audit && item.Entity is IAudit && item.Entity is BaseEntity entity)
            {
                var audit = this.Set<Audit>().Add(new Audit
                {
                    Id = this.NewGuid(),
                    EntityName = item.Entity.GetType().Name,
                    EntityId = entity.Id,
                    Action = item.State.ToString(),
                    TenantNumber = _tenantNumber,
                    CreatedOn = now,
                    CreatedBy = userName,
                }).Entity;
                if (item.State == EntityState.Added)
                {
                    audit.To = item.CurrentValues.ToObject().ToJson(jsonSerializerOptions);
                }
                else if (item.State == EntityState.Modified)
                {
                    audit.From = item.OriginalValues.ToObject().ToJson(jsonSerializerOptions);
                    audit.To = item.CurrentValues.ToObject().ToJson(jsonSerializerOptions);
                }
                else if (item.State == EntityState.Deleted)
                {
                    audit.From = item.OriginalValues.ToObject().ToJson(jsonSerializerOptions);
                    audit.To = item.CurrentValues.ToObject().ToJson(jsonSerializerOptions);
                }
            }
        }
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
        var type = this.GetType();
        var method = typeof(ModelBuilder).GetMethods().First(o => o.Name == nameof(modelBuilder.ApplyConfiguration));

        //添加实体配置
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract &&
            o.IsAssignableTo(typeof(ITenant)) &&
            o.GetCustomAttributes(typeof(DependsOnAttribute<>)).Any(o => o.GetType().GenericTypeArguments.First().IsAssignableTo(typeof(DbContext))))
            .ForEach(entityType =>
            {
                var dbContextType = entityType.GetCustomAttribute(typeof(DependsOnAttribute<>))!.GetType().GenericTypeArguments.First();
                if (dbContextType == type)
                {
                    //通用配置
                    ConfigEntity(modelBuilder, entityType);
                    //自定义配置
                    ServiceProvider.GetServices(typeof(IEntityTypeConfiguration<>).MakeGenericType(entityType)).ForEach(o => {
                        method.MakeGenericMethod(entityType).Invoke(modelBuilder, [o]);
                    });
                }
            });
    }

    private void ConfigEntity(ModelBuilder modelBuilder, Type entityType)
    {
        //配置实体
        var entityModlerBuilder = modelBuilder.Entity(entityType);
        if (entityType.IsAssignableTo(typeof(BaseEntity)))
        {
            //配置软删除、多租户的全局过滤器
            GetType().GetMethod(nameof(this.CreateQueryFilter))?.MakeGenericMethod(entityType).Invoke(this, [modelBuilder]);
            //配置实体Id
            entityModlerBuilder.HasKey(nameof(BaseEntity.Id));
            entityModlerBuilder.Property(nameof(BaseEntity.Id)).ValueGeneratedNever();
            //配置实体行版本号
            if (entityType.IsAssignableTo(typeof(IConcurrencyStampEntity)))
            {
                entityModlerBuilder.Property(nameof(IConcurrencyStampEntity.ConcurrencyStamp)).ValueGeneratedNever();
            }
            //配置NameNumber实体
            if (entityType.IsAssignableTo(typeof(BaseNameNumberEntity)))
            {
                entityModlerBuilder.Property(nameof(BaseNameNumberEntity.Name)).IsRequired();
                entityModlerBuilder.Property(nameof(BaseNameNumberEntity.Number)).IsRequired();
                entityModlerBuilder.HasIndex(nameof(ITenant.TenantNumber), nameof(BaseNameNumberEntity.Number)).IsUnique();//?
            }
            //配置单据实体
            if (entityType.GetBaseClasses().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseOrderEntity<>)))
            {
                entityModlerBuilder.Property(nameof(BaseOrderEntity<BaseEntity>.Number)).IsRequired();
                entityModlerBuilder.HasIndex(nameof(ITenant.TenantNumber), nameof(BaseOrderEntity<BaseEntity>.Number)).IsUnique();//?
            }
            if (entityType.GetBaseClasses().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseOrderItemEntity<>)))
            {
                entityModlerBuilder.HasOne(nameof(BaseOrderItemEntity<BaseEntity>.Order)).WithMany(nameof(BaseOrderEntity<BaseEntity>.Items)).HasForeignKey(nameof(BaseOrderItemEntity<BaseEntity>.OrderId)).OnDelete(DeleteBehavior.Cascade);
            }
            //配置树形结构实体
            if (entityType.IsAssignableTo(typeof(BaseTreeEntity<>).MakeGenericType(entityType)))
            {
                entityModlerBuilder.Property(nameof(BaseTreeEntity<BaseEntity>.Name)).IsRequired();
                entityModlerBuilder.Property(nameof(BaseTreeEntity<BaseEntity>.Number)).IsRequired();
                entityModlerBuilder.HasOne(nameof(BaseTreeEntity<BaseEntity>.Parent)).WithMany(nameof(BaseTreeEntity<BaseEntity>.Children)).HasForeignKey(nameof(BaseTreeEntity<BaseEntity>.ParentId)).OnDelete(DeleteBehavior.SetNull);
            }
            ////配置父子结构
            //if (entityType.GetBaseClasses().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == typeof(BaseChildTentity<>)))
            //{
            //    entityModlerBuilder.HasOne(nameof(BaseChildTentity<BaseEntity>.Parent)).WithMany().HasForeignKey(nameof(BaseChildTentity<BaseEntity>.ParentId)).OnDelete(DeleteBehavior.Cascade);
            //}
        }
        //配置属性
        var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
        properties.ForEach(prop =>
        {
            //配置只读字段（创建后不可更新）
            if (prop.Name != "Id" && prop.GetCustomAttributes<ReadOnlyAttribute>().Any())
            {
                entityModlerBuilder.Property(prop.Name).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            }
            //值对象非空
            if (prop.PropertyType.IsValueType && !prop.PropertyType.IsNullableType())
            {
                entityModlerBuilder.Property(prop.Name).IsRequired();
            }
            //配置枚举存储为字符串
            if (prop.PropertyType.GetUnderlyingType().IsEnum)
            {
                entityModlerBuilder.Property(prop.Name).HasConversion<string>();
            }
            //配置日期存取时为UTC时间
            if (prop.PropertyType.GetUnderlyingType() == typeof(DateTime))
            {
                if (prop.PropertyType.IsNullableType())
                {
                    entityModlerBuilder.Property<DateTime?>(prop.Name).HasConversion(v =>
                    v.HasValue ? v.Value.Kind == DateTimeKind.Utc ? v : v.Value.ToUniversalTime() : null,
                    v => v == null ? null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));
                }
                else
                {
                    entityModlerBuilder.Property<DateTime>(prop.Name).HasConversion(v =>
                    v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                }
            }
        });
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
}
