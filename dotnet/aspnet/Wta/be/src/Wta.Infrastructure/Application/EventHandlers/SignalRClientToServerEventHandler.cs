using Microsoft.AspNetCore.SignalR;
using Wta.Infrastructure.Application.Events;
using Wta.Infrastructure.Event;
using Wta.Infrastructure.SignalR;

namespace Wta.Infrastructure.Application.EventHandlers;

public class SignalRClientToServerEventHandler(IHubContext<DefaultHub> hubContext) : IEventHander<SignalRClientToServerEvent>
{
    public Task Handle(SignalRClientToServerEvent data)
    {
        Console.WriteLine(data);
        return Task.CompletedTask;
    }
}
