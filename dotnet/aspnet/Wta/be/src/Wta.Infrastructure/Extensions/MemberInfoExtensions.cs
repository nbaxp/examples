using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using Wta.Shared;

namespace Wta.Infrastructure.Extensions;

public static class MemberInfoExtensions
{
    public static string GetDisplayName(this MemberInfo memberInfo)
    {
        var scope = WebApp.Instance.WebApplication.Services.CreateScope();
        var localizer = scope?.ServiceProvider.GetService<IStringLocalizer>();
        var key = memberInfo.GetCustomAttribute<SwaggerOperationAttribute>()?.Summary ?? memberInfo.GetCustomAttribute<DisplayAttribute>()?.Name ?? memberInfo.Name;
        return localizer?.GetString(key, $"{memberInfo.ReflectedType!.Name}.{key}") ?? key;
    }
}
