using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class CaptchaController(ILogger<CaptchaController> logger,
    IHostEnvironment environment,
    IConfiguration configuration,
    IStringLocalizer stringLocalizer,
    IEncryptionService encryptionService,
    IImpageCaptchaService impageCaptchaService,
    IEmailService emailService,
    ISmsService smsService
   ) : BaseController
{
    [AllowAnonymous]
    [ResponseCache(NoStore = true)]
    public CustomApiResponse<CaptchaModel> Image()
    {
        var timeout = GetTimeout(configuration);
        var expires = DateTime.UtcNow.Add(timeout);
        var code = GetCode(4);
        return Json(new CaptchaModel
        {
            Expires = expires,
            AuthCode = $"data:image/png;charset=utf-8;base64,{Convert.ToBase64String(impageCaptchaService.Create(code))}",
            CodeHash = encryptionService.EncryptText($"{DateTime.UtcNow.Add(timeout)},{code}")
        });
    }

    [AllowAnonymous]
    [ResponseCache(NoStore = true)]
    public CustomApiResponse<CaptchaModel> Code([FromBody] string emailorPhone)
    {
        var timeout = GetTimeout(configuration);
        var expires = DateTime.UtcNow.Add(timeout);
        var code = GetCode(4);
        var isEmail = Regex.IsMatch(emailorPhone, @"\w+@\w+\.\w+");
        if (isEmail)
        {
            var subject = stringLocalizer.GetString("EmailCodeSubject");
            var body = stringLocalizer.GetString("EmailCodeBody", code);
            emailService.Send(subject, body, emailorPhone, emailorPhone);
        }
        else
        {
            smsService.Send(emailorPhone, out var code2);
            code = code2;
        }
        return Json(new CaptchaModel
        {
            Expires = expires,
            CodeHash = encryptionService.EncryptText($"{expires},{code},{emailorPhone}")
        }); ;
    }

    private static TimeSpan GetTimeout(IConfiguration configuration)
    {
        return configuration.GetValue<TimeSpan>("CaptchaTimeout", TimeSpan.Parse("00:05:00", CultureInfo.InvariantCulture));
    }

    private string GetCode(int count)
    {
        var code = "";
        for (int i = 0; i < count; i++)
        {
            code += RandomNumberGenerator.GetInt32(10);
        }
        if (environment.IsDevelopment())
        {
            logger.LogWarning($"captcha code: {code}");
        }
        return code;
    }
}
