using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Wta.Infrastructure.Module;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddModule<TApplicationModule>(this WebApplicationBuilder builder) where TApplicationModule : IApplicationModule, new()
    {
        var module = Activator.CreateInstance<TApplicationModule>();
        builder.Services.AddSingleton<IApplicationModule>(module);
        module.ConfigureModuleServices(builder);
        return builder;
    }
}