using Microsoft.Extensions.DependencyInjection;
using Wta.Infrastructure.Common;

namespace Wta.Infrastructure.Attributes;

public interface IImplementAttribute
{
    ServiceLifetime Lifetime { get; }
    Platform Platform { get; }
    Type ServiceType { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class ServiceAttribute<T> : Attribute, IImplementAttribute
{
    public ServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Transient, Platform platform = Platform.All)
    {
        ServiceType = typeof(T);
        Lifetime = lifetime;
        Platform = platform;
    }

    public Type ServiceType { get; }
    public ServiceLifetime Lifetime { get; }
    public Platform Platform { get; }
}
