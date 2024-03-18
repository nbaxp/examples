using Microsoft.AspNetCore.Mvc;
using Wta.Application.Default.Domain;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Web;

namespace Wta.Application.Default.Controllers;

public class MenuController(IRepository<Permission> menuRepository) : BaseController
{
    [Route("/api/menu")]
    [HttpPost]
    [AllowAnonymous]
    public CustomApiResponse<object> Menu()
    {
        var result = menuRepository.AsNoTracking().OrderBy(o => o.Order).ToList();
        return Json(result as object);
    }
}
