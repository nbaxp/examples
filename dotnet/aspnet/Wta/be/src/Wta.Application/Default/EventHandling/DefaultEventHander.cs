using Wta.Application.Default.Domain;
using Wta.Infrastructure.Interfaces;

namespace Wta.Application.Default.EventHandling;
public class DefaultEventHander(IRepository<User> repository) : IEventHander<EntityUpdatedEvent<User>>
{
    public Task Handle(EntityUpdatedEvent<User> data)
    {
        return Task.CompletedTask;
    }
}