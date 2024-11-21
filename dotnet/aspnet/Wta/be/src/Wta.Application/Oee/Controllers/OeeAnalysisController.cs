using Wta.Application.Oee.Domain;
using Wta.Application.Oee.Resources;

namespace Wta.Application.Platform.Controllers;

public class OeeAnalysisController(
    IRepository<OeeData> oeeDataRepository) : BaseController, IResourceService<OeeAnalysis>
{
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Schema()
    {
        return Json(typeof(OeeAnalysis).GetMetadataForType());
    }

    [Display(Name = "OEE分析")]
    [Authorize]
    [HttpPost]
    public ApiResult<List<OeeData>> Index(OeeAnalysis model)
    {
        if (!ModelState.IsValid)
        {
            throw new BadRequestException();
        }
        var data = oeeDataRepository.AsNoTracking()
            .Where(o => o.Date == model.Day)
            .Where(o => o.AssetNumber == model.AssetNumber)
            .OrderBy(o => o.Start)
            .ToList();
        return Json(data);
    }
}
