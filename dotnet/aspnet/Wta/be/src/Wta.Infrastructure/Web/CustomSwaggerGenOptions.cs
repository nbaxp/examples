using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Wta.Infrastructure.Web;

public class CustomSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiDescriptionGroupCollectionProvider provider;

    public CustomSwaggerGenOptions(IApiDescriptionGroupCollectionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiDescriptionGroups.Items)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo { Title = description.GroupName });
        }
    }
}

