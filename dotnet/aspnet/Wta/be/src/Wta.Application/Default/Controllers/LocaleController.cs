using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Wta.Application.Default.Models;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Web;

namespace Wta.Application.Default.Controllers;

[Route("api/locale")]
public class LocaleController : BaseController
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RequestLocalizationOptions _options;

    public LocaleController(IOptions<RequestLocalizationOptions> options, IServiceProvider serviceProvider)
    {
        _options = options.Value;
        _serviceProvider = serviceProvider;
    }

    [HttpPost]
    [AllowAnonymous]
    public CustomApiResponse<LocaleResponseModel> Index()
    {
        var result = new LocaleResponseModel
        {
            Locale = Thread.CurrentThread.CurrentCulture.Name,
            Messages = new Dictionary<string, object>(),
            Options = new List<KeyValuePair<string, string>>()
        };
        foreach (var item in _options.SupportedUICultures!)
        {
            result.Options.Add(new KeyValuePair<string, string>(item.Name, item.NativeName));
            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = item;
            var scope = _serviceProvider.CreateScope();
            var stringLocalizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer>();
            result.Messages.Add(item.Name, stringLocalizer.GetAllStrings().ToDictionary(o => o.Name, o => o.Value));
        }
        return Json(result);
    }
}