using System.Text;
using MQTTnet.AspNetCore;
using MQTTnet.Internal;
using MQTTnet.Server;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(o =>
{
    o.ListenAnyIP(1883, l => l.UseMqtt());
    // This will allow MQTT connections based on HTTP WebSockets with URI "localhost:5000/mqtt"
    // See code below for URI configuration.
    //o.ListenAnyIP(18083); // Default HTTP pipeline
});
builder.Services.AddHostedMqttServer(optionsBuilder =>
{
    optionsBuilder.WithDefaultEndpoint();
});

builder.Services.AddMqttConnectionHandler();
builder.Services.AddConnections();

var app = builder.Build();
app.UseMqttServer(
    server =>
    {
        /*
         * Attach event handlers etc. if required.
         */

        server.ValidatingConnectionAsync += ValidateConnectionAsync;
        server.ClientConnectedAsync += OnClientConnectedAsync;
        server.InterceptingPublishAsync += InterceptingPublishAsync;
    });

//app.UseRouting();

app.MapConnectionHandler<MqttConnectionHandler>("/mqtt",
    httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector = protocolList => protocolList.FirstOrDefault() ?? string.Empty);

app.MapGet("/", () => Results.File("~/index.html", "text/html"));

app.MapStaticAssets();

app.Run();
Task ValidateConnectionAsync(ValidatingConnectionEventArgs args)
{
    Console.WriteLine($"Client '{args.ClientId}' connected.");
    if (args.UserName != "admin" || args.Password != "aA123456!")
    {
        args.ReasonCode = MQTTnet.Protocol.MqttConnectReasonCode.BadUserNameOrPassword;
    }
    return CompletedTask.Instance;
}

Task OnClientConnectedAsync(ClientConnectedEventArgs args)
{
    Console.WriteLine($"Client '{args.ClientId}' wants to connect. Accepting!");
    return CompletedTask.Instance;
}

Task InterceptingPublishAsync(InterceptingPublishEventArgs arg)
{
    var payloadText = string.Empty;
    if (arg.ApplicationMessage.Payload.Length > 0)
    {
        payloadText = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
    }
    Console.WriteLine($"publish: '{arg.ClientId}' => {payloadText}");
    //写入 sqlite
    return CompletedTask.Instance;
}
