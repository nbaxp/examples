using MQTTnet.Server;

namespace Wta.Infrastructure.Mqtt;

[Service<MqttServerHandler>(ServiceLifetime.Singleton)]
public class MqttServerHandler(IServiceProvider serviceProvider)
{
    public async Task OnClientConnectedAsync(ClientConnectedEventArgs args)
    {
        Console.WriteLine($"Client '{args.ClientId}' wants to connect. Accepting!");
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public async Task ValidateConnectionAsync(ValidatingConnectionEventArgs args)
    {
        Console.WriteLine($"Client '{args.ClientId}' wants to connect. Accepting!");
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMqttServerService>();
        if (!service.Valid(args.ClientId, args.UserName, args.Password))
        {
            args.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
        }
        else
        {
            Console.WriteLine($"Client '{args.ClientId}' connected.");
        }
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public async Task InterceptingPublishAsync(InterceptingPublishEventArgs args)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IMqttServerService>();
        service.Receive(args.ClientId,args.ApplicationMessage);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
