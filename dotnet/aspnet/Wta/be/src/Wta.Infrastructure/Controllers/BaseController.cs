using Microsoft.AspNetCore.Mvc;
using Wta.Infrastructure.Web;

namespace Wta.Infrastructure.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseController : ControllerBase
{
    protected CustomApiResponse<T> Json<T>(T data, int code = 0, string? message = null)
    {
        return new CustomApiResponse<T> { Data = data, Code = code, Message = message };
    }
}
