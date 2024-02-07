using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;

namespace Wta.Application.Identity.Controllers;

public class RoleController(ILogger<Role> logger, IStringLocalizer stringLocalizer, IRepository<Role> repository, IExportImportService exportImportService) : GenericController<Role, RoleModel>(logger, stringLocalizer, repository, exportImportService)
{
    protected override void ToEntity(Role entity, RoleModel model, bool isCreate = false)
    {
        entity.RolePermissions.RemoveAll(o => !model.Permissions.Contains(o.PermissionId));
        entity.RolePermissions.AddRange(model.Permissions.Where(o => !entity.RolePermissions.Any(p => p.PermissionId == o)).Select(o => new RolePermission { PermissionId = o }));
    }
}
