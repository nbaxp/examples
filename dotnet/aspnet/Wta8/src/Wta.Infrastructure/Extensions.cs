using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Wta.Infrastructure;

public static class Extensions
{
  public static WebApplicationBuilder AddApplication<TApplication>(this WebApplicationBuilder builder)
    where TApplication : class, IApplication
  {
    var application = Activator.CreateInstance<TApplication>();
    application.ConfigureServices(builder);
    builder.Services.AddSingleton(application);
    builder.Services.AddSingleton<IApplication>(application);
    return builder;
  }

  public static WebApplicationBuilder AddModule<TModule>(this WebApplicationBuilder builder)
    where TModule : class, IModule
  {
    var module = Activator.CreateInstance<TModule>();
    module.ConfigureServices(builder);
    builder.Services.AddSingleton(module);
    builder.Services.AddSingleton<IModule>(module);
    return builder;
  }

  public static WebApplication UseModules(this WebApplication app)
  {
    app.Services.GetRequiredService<IApplication>().Configure(app);
    foreach (var item in app.Services.GetServices<IModule>())
    {
      item.Configure(app);
    }
    return app;
  }
}
