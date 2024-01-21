using System.Globalization;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class CaptchaController(IConfiguration configuration, IEncryptionService encryptionService, IImpageCaptchaService impageCaptchaService) : BaseController
{
    [AllowAnonymous]
    [ResponseCache(NoStore = true)]
    public CustomApiResponse<CaptchaModel> Image()
    {
        var timeout = configuration.GetValue<TimeSpan>("CaptchaTimeout", TimeSpan.Parse("00:05:00", CultureInfo.InvariantCulture));
        var code = GetCode(4);
        return Json(new CaptchaModel
        {
            Code = $"data:image/png;charset=utf-8;base64,{Convert.ToBase64String(impageCaptchaService.Create(code))}",
            CodeHash = encryptionService.EncryptText($"{DateTime.UtcNow.Add(timeout)},{code}")
        }); ;
    }

    private static string GetCode(int count)
    {
        var code = "";
        for (int i = 0; i < count; i++)
        {
            code += RandomNumberGenerator.GetInt32(10);
        }
        return code;
    }
}
