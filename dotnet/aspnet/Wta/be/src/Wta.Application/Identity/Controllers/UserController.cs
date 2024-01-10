using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class UserController(ILogger<User> logger, IRepository<User> repository, IExportImportService exportImportService) : GenericController<User, UserModel>(logger, repository, exportImportService)
{
    [Authorize]
    public CustomApiResponse<UserModel> Info()
    {
        var result = Repository
            .AsNoTracking()
            .Include(o => o.UserRoles)
            .ThenInclude(o => o.Role)
            .FirstOrDefault(o => o.UserName == User.Identity!.Name)!;

        return Json(new UserModel
        {
            UserName = result.UserName,
            Name = result.Name,
            Avatar = result.Avatar,
            Roles = result.UserRoles.Select(o => o.Role!).Select(o => new RoleModel { Name = o.Name, Number = o.Number }).ToList()
        });
    }
}
