using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Application;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class UserController(ILogger<User> logger, IRepository<User> repository, IMapper<User, UserModel> mapper, IExportImportService exportImportService, IEncryptionService passwordHasher) : GenericController<User, UserModel>(logger, repository, mapper, exportImportService)
{
    [Authorize, Hidden]
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
            //Roles = result.UserRoles.Select(o => o.Role!).Select(o => new RoleModel { Name = o.Name, Number = o.Number }).ToList()
        });
    }

    [AllowAnonymous, Hidden]
    public CustomApiResponse<bool> ValidUserName(string userName)
    {
        var normalizedUserName = userName.ToUpperInvariant();
        return Json(!Repository.AsNoTracking().Any(o => o.NormalizedUserName == normalizedUserName));
    }

    [AllowAnonymous, Hidden]
    public CustomApiResponse<bool> Register(RegisterRequestModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User();
            Mapper.FromObject(user, model);
            user.NormalizedUserName = user.UserName!.ToUpperInvariant();
            user.SecurityStamp = passwordHasher.CreateSalt();
            user.PasswordHash = passwordHasher.HashPassword(model.Password!, user.SecurityStamp);
        }
        throw new BadRequestException();
    }
}
