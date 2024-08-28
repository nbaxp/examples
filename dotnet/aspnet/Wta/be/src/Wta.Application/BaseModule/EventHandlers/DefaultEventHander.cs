namespace Wta.Application.BaseModule.EventHandling;
public class DefaultEventHander(IRepository<User> repository) : IEventHander<EntityUpdatedEvent<User>>
{
    public Task Handle(EntityUpdatedEvent<User> data)
    {
        return Task.CompletedTask;
    }
}
