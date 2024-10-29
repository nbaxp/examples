
namespace Wta.Application.Platform.Controllers;

[View("user-center/message")]
public class MessageController : BaseController, IResourceService<Message>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Index()
    {
        return Json(typeof(ResetPasswordModel).GetMetadataForType());
    }
}
