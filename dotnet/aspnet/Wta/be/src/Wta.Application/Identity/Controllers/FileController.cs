using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class FileController(IWebHostEnvironment environment) : BaseController
{
    [Authorize]
    public CustomApiResponse<string> Upload(IFormFile file)
    {
        var folder = "api/upload";
        using Stream stream = file.OpenReadStream();
        var ext = Path.GetExtension(file.FileName);
        var md5 = GetFileNameHash(stream);
        var phicyPath = Path.Combine(environment.WebRootPath, folder);
        Directory.CreateDirectory(phicyPath);
        var name = string.Format(CultureInfo.CurrentCulture, "{0}{1}", md5, ext);
        var fullName = Path.Combine(phicyPath, name);
        if (!System.IO.File.Exists(fullName))
        {
            using (FileStream fs = System.IO.File.Create(fullName))
            {
                file.CopyTo(fs);
            }
        }

        return Json($"{folder}/{name}");
    }

    private static string GetFileNameHash(Stream input)
    {
        using var hashAlg = SHA256.Create();
        byte[] hash = hashAlg.ComputeHash(input);
        return BitConverter.ToString(hash).Replace("-", string.Empty, true, CultureInfo.CurrentCulture).ToLower(CultureInfo.CurrentCulture);
    }
}
