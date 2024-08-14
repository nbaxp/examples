using Wta.Infrastructure.Mapper;

namespace Wta.Application.System.Controllers;

public class RoleController(ILogger<Role> logger, IStringLocalizer stringLocalizer, IObjerctMapper mapper, IRepository<Role> repository, IEventPublisher eventPublisher, IExportImportService exportImportService) : GenericController<Role, Role>(logger, stringLocalizer, mapper, repository, eventPublisher, exportImportService)
{
    protected override void ToModel(Role entity, Role model)
    {
        model.Permissions = entity.RolePermissions.Select(o => o.PermissionId).ToList();
    }

    protected override void ToEntity(Role entity, Role model, bool isCreate)
    {
        entity.RolePermissions.Clear();
        entity.RolePermissions.AddRange(model.Permissions.Select(o => new RolePermission { PermissionId = o }));
    }
}
