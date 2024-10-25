using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Wta.Infrastructure.Module;

public static class WebApplicationExtensions
{
    public static WebApplication UseModules(this WebApplication app)
    {
        foreach (var item in app.Services.GetServices<IApplicationModule>())
        {
            item.ConfigureModule(app);
        }
        return app;
    }
}