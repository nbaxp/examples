using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wta.Application.Identity.Domain;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class MenuController : BaseController
{
    private readonly IRepository<Permission> _menuRepository;

    public MenuController(IRepository<Permission> menuRepository)
    {
        _menuRepository = menuRepository;
    }

    [Route("/api/menu")]
    [HttpPost]
    [AllowAnonymous]
    public CustomApiResponse<object> Menu()
    {
        var result = _menuRepository.AsNoTracking().OrderBy(o => o.Order).ToList();
        return Json(result as object);
    }
}
