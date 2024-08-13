namespace Wta.Application.Default.Controllers;

[View("user-center/reset-password")]
public class ResetPasswordController(IRepository<User> repository, IEncryptionService encryptionService) : BaseController, IResourceService<ResetPasswordModel>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Index()
    {
        return Json(typeof(ResetPasswordModel).GetMetadataForType());
    }

    [Authorize, Ignore]
    public ApiResult<bool> Index(ResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
            var user = repository.Query().FirstOrDefault(o => o.NormalizedUserName == normalizedUserName)!;
            if (user != null)
            {
                if (user.PasswordHash != encryptionService.HashPassword(model.CurrentPassword!, user.SecurityStamp!))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "WrongPassword");
                }
                else
                {
                    user.PasswordHash = encryptionService.HashPassword(model.NewPassword!, user.SecurityStamp!);
                    repository.SaveChanges();
                    return Json(true);
                }
            }
            else
            {
                throw new ProblemException("UserNotExist");
            }
        }
        throw new BadRequestException();
    }
}
