using Mapster;
using Microsoft.AspNetCore.Mvc;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Models;
using Wta.Infrastructure.Web;
using Wta.Shared;

namespace Wta.Application.Identity.Controllers;

public class TenantController(ILogger<Tenant> logger,
    IStringLocalizer stringLocalizer,
    IRepository<Tenant> repository,
    IRepository<Role> roleRepository,
    IExportImportService exportImportService,
    ITenantService tenantService,
    IServiceProvider serviceProvider) : GenericController<Tenant, TenantModel>(logger, stringLocalizer, repository, exportImportService)
{
    [Ignore]
    public override FileContentResult ImportTemplate()
    {
        return base.ImportTemplate();
    }

    [Ignore]
    public override CustomApiResponse<bool> Import(ImportModel<TenantModel> model)
    {
        return base.Import(model);
    }

    [Ignore]
    public override FileContentResult Export(ExportModel<TenantModel> model)
    {
        return base.Export(model);
    }

    [Ignore]
    public override CustomApiResponse<bool> Delete([FromBody] Guid[] items)
    {
        return base.Delete(items);
    }

    [AllowAnonymous]
    public override CustomApiResponse<QueryModel<TenantModel>> Search(QueryModel<TenantModel> model)
    {
        return base.Search(model);
    }

    public override CustomApiResponse<bool> Create(TenantModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        //创建租户
        var entity = new Tenant().FromModel(model, o => o.Ignore(o => o.Id)).SetIdBy(o => o.Number);
        Repository.Add(entity);
        Repository.SaveChanges();
        //切换为租户
        tenantService.TenantId = entity.Id;
        //初始化租户
        using var scope = serviceProvider.CreateScope();
        //设置租户Id
        scope.ServiceProvider.GetRequiredService<ITenantService>().TenantId = entity.Id;
        //设置租户种子数据
        WebApp.Instance.Assemblies
        .SelectMany(o => o.GetTypes())
        .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
        .OrderBy(o => o.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0)
        .ForEach(dbContextType =>
        {
            var contextName = dbContextType.Name;
            if (scope.ServiceProvider.GetRequiredService(dbContextType) is DbContext dbContext)
            {
                var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
                scope.ServiceProvider.GetServices(dbSeedType).ForEach(o => dbSeedType.GetMethod("Seed")?.Invoke(o, new object[] { dbContext }));
            }
        });
        return Json(true);
    }

    public override CustomApiResponse<TenantModel> Details([FromBody] Guid id)
    {
        var entity = Repository.AsNoTracking().FirstOrDefault(o => o.Id == id);
        if (entity == null)
        {
            throw new ProblemException("NotFound");
        }
        var model = entity.ToModel<Tenant, TenantModel>();
        roleRepository.DisableTenantFilter();
        var rolePermissions =
        model.Permissions = roleRepository.AsNoTracking()
        .Include(o => o.RolePermissions)
        .Where(o => o.Number == "admin" && o.TenantId == id)
        .SelectMany(o => o.RolePermissions)
        .Select(o => o.PermissionId)
        .ToList();
        return Json(model);
    }

    public override CustomApiResponse<bool> Update([FromBody] TenantModel model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var entity = Repository.Query().First(o => o.Id == model.Id);
        entity.FromModel(model, o => o.Ignore(o => o.Id).IgnoreAttribute(typeof(ReadOnlyAttribute)));
        roleRepository.DisableTenantFilter();
        var tenantRole = roleRepository.Query()
            .Include(o => o.RolePermissions.Where(o => o.Role!.TenantId == entity.Id))
            .First(o => o.Number == "admin" && o.TenantId == entity.Id);
        tenantRole.RolePermissions.RemoveAll(o => !model.Permissions.Contains(o.PermissionId));
        tenantRole.RolePermissions.AddRange(model.Permissions.Where(o => !tenantRole.RolePermissions.Any(p => p.PermissionId == o)).Select(o => new RolePermission { PermissionId = o }));
        Repository.SaveChanges();
        return Json(true);
    }

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> NoName([FromForm] string name)
    {
        return Json(!Repository.AsNoTracking().Any(o => o.Name == name));
    }

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> NoNumber([FromForm] string number)
    {
        return Json(!Repository.AsNoTracking().Any(o => o.Number == number));
    }
}
