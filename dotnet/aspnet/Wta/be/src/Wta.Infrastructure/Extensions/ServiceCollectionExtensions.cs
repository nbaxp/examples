namespace Wta.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDbContext(this IServiceCollection serviceCollection, Type dbContextType, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    {
        typeof(EntityFrameworkServiceCollectionExtensions)
            .InvokeExtensionMethod("AddDbContext", [dbContextType], null!, serviceCollection, optionsAction, contextLifetime, optionsLifetime);
    }

    public static IServiceCollection Clone(this IServiceCollection serviceCollection)
    {
        IServiceCollection clone = new ServiceCollection();
        foreach (ServiceDescriptor service in serviceCollection)
        {
            clone.Add(service);
        }

        return clone;
    }
}
