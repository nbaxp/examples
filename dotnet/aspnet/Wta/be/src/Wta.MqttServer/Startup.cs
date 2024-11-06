using MQTTnet.AspNetCore;
using MQTTnet.Server;

namespace Wta.MqttServer;

public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                        protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            });

        app.UseMqttServer(
            server =>
            {
                server.ValidatingConnectionAsync += ValidateConnection;
                server.ClientConnectedAsync += OnClientConnected;
                server.ClientDisconnectedAsync += OnClientDisconnected;
            });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHostedMqttServer(
                        optionsBuilder =>
                        {
                            optionsBuilder.WithDefaultEndpoint();
                        });

        services.AddMqttConnectionHandler();
        services.AddConnections();
    }

    private Task OnClientConnected(ClientConnectedEventArgs args)
    {
        Console.WriteLine($"客户端 '{args.ClientId}' 已连接.");
        return Task.CompletedTask;
    }

    private Task OnClientDisconnected(ClientDisconnectedEventArgs args)
    {
        Console.WriteLine($"客户端 '{args.ClientId}' 已断开.");
        return Task.CompletedTask;
    }

    private Task ValidateConnection(ValidatingConnectionEventArgs args)
    {
        Console.WriteLine($"客户端 '{args.ClientId}' 连接中");
        if (args.UserName == "" && args.Password == "")
        {
        }
        else
        {
            args.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
        }
        return Task.CompletedTask;
    }
}
