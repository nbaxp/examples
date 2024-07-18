namespace Wta.Controllers;

[AllowAnonymous, View("home"), Icon("home")]
public class HomeController : Controller, IResourceService<HomeModel>
{
    [ResponseCache(NoStore = true)]
    public IActionResult Index()
    {
        return File("~/index.html", "text/html");
    }
}
