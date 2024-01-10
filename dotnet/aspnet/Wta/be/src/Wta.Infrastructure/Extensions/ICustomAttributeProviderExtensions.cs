using System.Reflection;

namespace Wta.Infrastructure.Extensions;

public static class ICustomAttributeProviderExtensions
{
    public static T? GetAttribute<T>(this ICustomAttributeProvider customAttributeProvider) where T : Attribute
    {
        return customAttributeProvider.GetCustomAttributes(typeof(T), true).FirstOrDefault() as T;
    }
}
