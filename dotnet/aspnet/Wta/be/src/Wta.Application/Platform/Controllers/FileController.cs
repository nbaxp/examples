using Wta.Infrastructure.FileProviders;

namespace Wta.Application.Platform.Controllers;

public class FileController(IFileService fileService) : BaseController
{
    [HttpGet, Route("/api/file/{name}"), AllowAnonymous]
    public IActionResult Index(string name)
    {
        return fileService.Download(name);
    }

    [Authorize]
    public ApiResult<string> Upload(IFormFile file)
    {
        return Json($"api/file/{fileService.Upload(file)}");
    }
}
