using System.Text;
using System.Text.Json;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.EntityFrameworkCore;
using MQTTnet.AspNetCore;
using MQTTnet.Internal;
using MQTTnet.Server;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));

builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connectionString).UseSeeding((context, _) =>
{
    //context.Set<>().Add();
    context.SaveChanges();
}));
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
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

async Task InterceptingPublishAsync(InterceptingPublishEventArgs arg)
{
    try
    {
        var payloadText = string.Empty;
        if (arg.ApplicationMessage.Payload.Length > 0)
        {
            payloadText = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);
        }
        Console.WriteLine($"publish: '{arg.ClientId}' => {payloadText}");
        using var client = new InfluxDBClient("http://localhost:8086",
                                "admin",
                                "aA123456",
                                "wta",
                                "autogen");
        var list = new List<PointData>();
        var point = PointData.Builder.Measurement("device_data")
            .Timestamp(DateTime.UtcNow, InfluxDB.Client.Api.Domain.WritePrecision.Ns)
            .Tag("topic", arg.ApplicationMessage.Topic)
            .Tag("message", payloadText);
        var paths = arg.ApplicationMessage.Topic.Split('/');
        for (var i = 0; i < paths.Length; i++)
        {
            var path = paths[i].Trim();
            point.Tag($"path{i + 1}", path);
        }
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadText);
            if (dict != null)
            {
                foreach (var kvp in dict)
                {
                    point.Field(kvp.Key, kvp.Value);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        list.Add(point.ToPointData());
        var writeApi = client.GetWriteApiAsync();
        await writeApi.WritePointsAsync(list).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}
