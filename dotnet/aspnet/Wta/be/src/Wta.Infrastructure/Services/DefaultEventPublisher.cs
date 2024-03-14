using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Interfaces;

namespace Wta.Infrastructure.Services;

[Service<IEventPublisher>(ServiceLifetime.Singleton)]
public class DefaultEventPublisher(IServiceProvider serviceProvider) : IEventPublisher
{
    public async Task Publish<T>(T data)
    {
        var subscribers = serviceProvider.GetServices<IEventHander<T>>().ToList();
        foreach (var item in subscribers)
        {
            await item.Handle(data).ConfigureAwait(false);
        }
    }
}
