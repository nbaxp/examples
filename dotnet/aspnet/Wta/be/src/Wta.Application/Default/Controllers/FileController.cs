using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Default.Controllers;

public class FileController(IFileService fileService) : BaseController
{
    [HttpGet, Route("/api/file/{name}"), AllowAnonymous]
    public IActionResult Index(string name)
    {
        return fileService.Download(name);
    }

    [Authorize]
    public CustomApiResponse<string> Upload(IFormFile file)
    {
        return Json($"api/file/{fileService.Upload(file)}");
    }
}