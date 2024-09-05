namespace Wta.Application.SystemModule.EventHandling;
public class DefaultEventHander(IRepository<User> repository) : IEventHander<EntityUpdatedEvent<User>>
{
    public Task Handle(EntityUpdatedEvent<User> data)
    {
        return Task.CompletedTask;
    }
}
