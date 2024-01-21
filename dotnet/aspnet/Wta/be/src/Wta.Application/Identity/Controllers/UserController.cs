using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public CustomApiResponse<UserInfoModel> Info()
    {
        var result = Repository
            .AsNoTracking()
            .Include(o => o.UserRoles)
            .ThenInclude(o => o.Role)
            .ThenInclude(o => o!.RolePermissions)
            .ThenInclude(o => o.Permission)
            .FirstOrDefault(o => o.UserName == User.Identity!.Name)!;

        return Json(new UserInfoModel
        {
            UserName = result.UserName,
            Name = result.Name,
            Avatar = result.Avatar,
            Roles = result.UserRoles.Select(o => o.Role!.Number).ToList(),
            Permissions = result.UserRoles.SelectMany(o => o.Role!.RolePermissions).Select(o => o.Permission!.Path).ToList()
        });
    }

    [AllowAnonymous, Hidden]
    public CustomApiResponse<bool> ValidUserName([FromForm] string userName)
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

    public CustomApiResponse<bool> ResetPassword(ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = Repository.Query().FirstOrDefault(o => o.Name == User.Identity!.Name)!;
            if (user.PasswordHash != passwordHasher.HashPassword(model.OldPassword!, user.SecurityStamp!))
            {
                ModelState.AddModelError(nameof(model.OldPassword), "WrongPassword");
            }
            else
            {
                user.PasswordHash = passwordHasher.HashPassword(model.Password!, user.SecurityStamp!);
                Repository.SaveChanges();
                return Json(true);
            }
        }
        throw new BadRequestException();
    }
}
