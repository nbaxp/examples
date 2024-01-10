using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Wta.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddDbContext(this IServiceCollection serviceCollection, Type dbContextType, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
    {
        typeof(EntityFrameworkServiceCollectionExtensions)
            .InvokeExtensionMethod("AddDbContext", [dbContextType], null!, serviceCollection, optionsAction, contextLifetime, optionsLifetime);
    }
}
