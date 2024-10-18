using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.SystemModule.Controllers;

public class TenantController(ILogger<Tenant> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Tenant> repository,
    IEventPublisher eventPublisher,
    IRepository<Role> roleRepository,
    IRepository<Permission> permissionRepository,
    IExportImportService exportImportService,
    IServiceProvider serviceProvider) : GenericController<Tenant, Tenant>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    [Ignore]
    public override FileContentResult ImportTemplate()
    {
        return base.ImportTemplate();
    }

    [Ignore]
    public override ApiResult<bool> Import(ImportModel<Tenant> model)
    {
        return base.Import(model);
    }

    [Ignore]
    public override FileContentResult Export(ExportModel<Tenant> model)
    {
        return base.Export(model);
    }

    [Ignore]
    public override ApiResult<bool> Delete([FromBody] Guid[] items)
    {
        return base.Delete(items);
    }

    [AllowAnonymous]
    public override ApiResult<QueryModel<Tenant>> Search(QueryModel<Tenant> model)
    {
        return base.Search(model);
    }

    public override ApiResult<bool> Create(Tenant model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        //创建租户
        var entity = ObjectMapper.ToEntity<Tenant, Tenant>(model).SetIdBy(o => o.Number);
        Repository.Add(entity);
        Repository.SaveChanges();
        //初始化租户
        using var scope = serviceProvider.CreateScope();
        //设置租户Id
        var tenantService = scope.ServiceProvider.GetRequiredService<ITenantService>();
        tenantService.TenantNumber = entity.Number;
        tenantService.Permissions = model.Permissions;
        //设置租户种子数据
        AppDomain.CurrentDomain.GetCustomerAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(o => o.IsClass && !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
            .OrderBy(o => o.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0)
            .ForEach(dbContextType =>
            {
                var contextName = dbContextType.Name;
                if (scope.ServiceProvider.GetRequiredService(dbContextType) is DbContext dbContext)
                {
                    var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
                    scope.ServiceProvider.GetServices(dbSeedType).ForEach(o =>
                    {
                        dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(o, [dbContext]);
                    });
                }
            });
        return Json(true);
    }

    public override ApiResult<Tenant> Details([FromBody] Guid id)
    {
        var entity = Repository.AsNoTracking().FirstOrDefault(o => o.Id == id) ?? throw new ProblemException("NotFound");
        var model = ObjectMapper.ToModel<Tenant, Tenant>(entity, (entity, model) =>
        {
            roleRepository.DisableTenantFilter();
            var rolePermissions =
            model.Permissions = [.. roleRepository.AsNoTracking()
            .Where(o => o.Number == "admin" && o.TenantNumber == entity.Number)
            .SelectMany(o => o.RolePermissions)
            .Select(o => o.Permission!.Number)];
        });
        return Json(model);
    }

    public override ApiResult<bool> Update([FromBody] Tenant model)
    {
        Repository.DisableTenantFilter();
        var entity = Repository.Query().First(o => o.Id == model.Id);
        ObjectMapper.FromModel(entity, model);
        Repository.SaveChanges();
        var tenantRole = roleRepository.Query()
            .Include(o => o.RolePermissions)
            .First(o => o.Number == "admin" && o.TenantNumber == entity.Number);
        var permissions = permissionRepository.Query().Where(o => o.TenantNumber == entity.Number).ToList();
        tenantRole.RolePermissions.Clear();
        Repository.SaveChanges();
        tenantRole.RolePermissions.AddRange(permissions.Where(o => model.Permissions.Any(p => o.Number == p))
            .Select(o => new RolePermission { PermissionId = o.Id, RoleId = tenantRole.Id, TenantNumber = entity.TenantNumber })
            .ToList());
        permissions.Where(o => !o.Disabled && !model.Permissions.Any(p => o.Number == p)).ForEach(o => o.Disabled = true);
        permissions.Where(o => o.Disabled && model.Permissions.Any(p => o.Number == p)).ForEach(o => o.Disabled = false);
        Repository.SaveChanges();
        return Json(true);
    }

    protected override void ToModel(Tenant entity, Tenant model)
    {
        model.Permissions = entity.Permissions;
    }

    [AllowAnonymous, Ignore]
    public ApiResult<bool> NoName([FromForm] string name)
    {
        return Json(!Repository.AsNoTracking().Any(o => o.Name == name));
    }

    [AllowAnonymous, Ignore]
    public ApiResult<bool> NoNumber([FromForm] string number)
    {
        return Json(!Repository.AsNoTracking().Any(o => o.Number == number));
    }
}
