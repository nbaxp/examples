namespace Wta.Controllers;

[View("home"), Icon("home")]
[AllowAnonymous]
public class HomeController : Controller, IResourceService<HomeModel>
{
    [ResponseCache(NoStore = true), Ignore]
    public IActionResult Index()
    {
        return File("~/index.html", "text/html");
    }
}
