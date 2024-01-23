using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

[Service<IAuthService>(ServiceLifetime.Transient)]
public class UserController(ILogger<User> logger, IRepository<User> repository, IMapper<User, UserModel> mapper, IExportImportService exportImportService, IHttpContextAccessor httpContextAccessor, IEncryptionService passwordHasher) : GenericController<User, UserModel>(logger, repository, mapper, exportImportService), IAuthService
{
    [Authorize, Ignore]
    public bool HasPermission(string permission)
    {
        var userName = httpContextAccessor.HttpContext!.User.Identity!.Name;
        return Repository.AsNoTracking()
            .Any(o => o.UserName == userName && o.UserRoles.Any(o => o.Role!.RolePermissions.Any(o => o.Permission!.Type == MenuType.Button && o.Permission!.Path == permission)));
    }

    [Authorize, Ignore]
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

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> ValidUserName([FromForm] string userName)
    {
        var normalizedUserName = userName.ToUpperInvariant();
        return Json(!Repository.AsNoTracking().Any(o => o.NormalizedUserName == normalizedUserName));
    }

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> Register(EmailRegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var values = passwordHasher.DecryptText(model.CodeHash!).Split(',');
            if (values[2] == model.Email)
            {
                CreateUser(model, o => o.EmailConfirmed = true);
                return Json(true);
            }
            ModelState.AddModelError(nameof(model.Email), "EmailError");
        }
        throw new BadRequestException();
    }

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> SmsRegister(SmsRegisterModel model)
    {
        if (ModelState.IsValid)
        {
            var values = passwordHasher.DecryptText(model.CodeHash!).Split(',');
            if (values[2] == model.PhoneNumber)
            {
                CreateUser(model, o => o.EmailConfirmed = true);
                return Json(true);
            }
            ModelState.AddModelError(nameof(model.PhoneNumber), "PhoneNumberError");
        }
        throw new BadRequestException();
    }

    [AllowAnonymous, Ignore]
    public CustomApiResponse<bool> ForgotPassword(ForgotPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var values = passwordHasher.DecryptText(model.CodeHash!).Split(',');
            if (values[2] == model.EmailOrPhoneNumber)
            {
                var user = Repository.Query().FirstOrDefault(o => o.UserName == model.EmailOrPhoneNumber || o.PhoneNumber == model.EmailOrPhoneNumber)!;
                user.PasswordHash = passwordHasher.HashPassword(model.Password!, user.SecurityStamp!);
                Repository.SaveChanges();
                return Json(true);
            }
            ModelState.AddModelError(nameof(model.EmailOrPhoneNumber), "EmailOrPhoneNumberError");
        }
        throw new BadRequestException();
    }

    [AllowAnonymous, Ignore]
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

    private void CreateUser(RegisterModelBase model, Action<User> action)
    {
        var user = new User();
        Mapper.FromObject(user, model);
        user.NormalizedUserName = user.UserName!.ToUpperInvariant();
        user.SecurityStamp = passwordHasher.CreateSalt();
        user.PasswordHash = passwordHasher.HashPassword(model.Password!, user.SecurityStamp);
        action.Invoke(user);
        Repository.Add(user);
        Repository.SaveChanges();
    }
}
