namespace Wta.Application.Platform.Controllers;

[AllowAnonymous]
public class HomeController(IServiceProvider serviceProvider) : Controller
{
    [ResponseCache(NoStore = true), Ignore]
    public IActionResult Index()
    {
        return File("~/index.html", "text/html");
    }

    [Ignore]
    public string Reset()
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            if (env.IsDevelopment())
            {
            }
            var list = scope.ServiceProvider.GetServices<DbContext>();
            foreach (var item in list)
            {
                item.Database.EnsureDeleted();
                item.Database.EnsureCreated();
                var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(item.GetType());
                var seedList = scope.ServiceProvider.GetServices(dbSeedType)
                .OrderBy(o => o!.GetType().GetAttribute<DisplayAttribute>()?.GetOrder() ?? 0)
                .ToList();
                foreach (var seeder in seedList)
                {
                    dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(seeder, [item]);
                }
            }
            return "OK";
        }
        catch (Exception ex)
        {
            return ex.Message + ex.ToString();
        }
    }

    //public object Test()
    //{
    //    var mqttclient = serviceProvider.GetRequiredService<MqttHostedService>().Client;
    //    return mqttclient.TryPingAsync().Result;
    //}
}
