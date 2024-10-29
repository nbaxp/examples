namespace Wta.Application.Platform.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    [ResponseCache(NoStore = true), Ignore]
    public IActionResult Index()
    {
        return File("~/index.html", "text/html");
    }
}
