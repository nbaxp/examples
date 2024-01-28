//using Microsoft.Extensions.Logging;
//using Wta.Application.Identity.Domain;
//using Wta.Application.Identity.Models;
//using Wta.Infrastructure.Controllers;
//using Wta.Infrastructure.Interfaces;

//namespace Wta.Application.Identity.Controllers;

//public class RoleController(ILogger<Role> logger, IRepository<Role> repository, IExportImportService exportImportService) : GenericController<Role, RoleModel>(logger, repository, exportImportService)
//{
//    protected override void ToModel(Role entity, RoleModel model)
//    {
//        model.Permissions = entity.RolePermissions.Select(o => o.PermissionId).ToList ();
//    }

//    protected override void ToEntity(Role entity, RoleModel model)
//    {
//        entity.RolePermissions = model.Permissions.Select(o => new RolePermission { PermissionId = o }).ToList();
//    }
//}
