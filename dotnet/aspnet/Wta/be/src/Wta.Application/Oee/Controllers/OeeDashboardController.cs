using Wta.Application.Oee.Models;
using Wta.Application.Oee.Resources;

namespace Wta.Application.Platform.Controllers;

public class OeeDashboardController() : BaseController, IResourceService<OeeDashboard>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Schema()
    {
        return Json(typeof(OeeDashboard).GetMetadataForType());
    }

    [Display(Name = "OEE仪表盘")]
    [Authorize]
    [HttpPost]
    public ApiResult<OeeDashboardResult> Index()
    {
        var result = new OeeDashboardResult();
        result.Oee = 0.70f;
        return Json(result);
    }
}
