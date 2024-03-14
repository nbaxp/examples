using Microsoft.AspNetCore.SignalR;
using Wta.Infrastructure.Events;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Infrastructure.EventHandlers;

public class SignalRClientToServerEventHandler(IHubContext<DefaultHub> hubContext) : IEventHander<SignalRClientToServerEvent>
{
    public Task Handle(SignalRClientToServerEvent data)
    {
        Console.WriteLine(data);
        return Task.CompletedTask;
    }
}
