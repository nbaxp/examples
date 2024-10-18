using Wta.Infrastructure.Application.Models;
using Wta.Infrastructure.Mapper;

namespace Wta.Application.SystemModule.Controllers;

public class PermissionController(ILogger<Permission> logger,
    IStringLocalizer stringLocalizer,
    IObjerctMapper mapper,
    IRepository<Permission> repository,
    IEventPublisher eventPublisher,
    IExportImportService exportImportService) : GenericController<Permission, Permission>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override void ToModel(Permission entity, Permission model)
    {
        model.RolesList = entity.Roles?.Split(',').ToList();
    }

    protected override void ToEntity(Permission entity, Permission model)
    {
        if (entity.AuthType == AuthType.Roles)
        {
            entity.Roles = model.RolesList != null ? string.Join(",", model.RolesList) : null;
        }
        else
        {
            entity.Roles = null;
        }
    }

    [Hidden]
    [AllowAnonymous]
    public ApiResult<QueryModel<Permission>> Tenant()
    {
        var result = new QueryModel<Permission> { Items = Repository.AsNoTracking().Where(o => !o.Disabled).ToList() };
        return Json(result);
    }
}
