using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MQTTnet.AspNetCore;
using MQTTnet.Internal;
using MQTTnet.Server;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

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
            payloadText = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload).Trim();
            payloadText = Regex.Replace(payloadText, @"\s+", " ");
        }
        Console.WriteLine($"publish: '{arg.ClientId}' => {payloadText}");
        using var client = new InfluxClient(new Uri("http://localhost:8086"), "admin", "aA123456");
        var dbName = "wta";
        var measurementName = "device";
        await client.CreateDatabaseAsync(dbName).ConfigureAwait(false);
        var row = new DynamicInfluxRow
        {
            Timestamp = DateTime.UtcNow
        };
        row.SetField("topic", arg.ApplicationMessage.Topic);
        row.SetField("message", payloadText);
        var paths = arg.ApplicationMessage.Topic.Split('/');
        for (var i = 0; i < paths.Length; i++)
        {
            var path = paths[i].Trim();
            row.SetTag($"path{i + 1}", path);
        }
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(payloadText);
            if (dict != null)
            {
                foreach (var kvp in dict)
                {
                    var value = GetValue(kvp.Value);
                    if (value != null)
                    {
                        row.SetField(kvp.Key, value);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        await client.WriteAsync(dbName, measurementName, [row]).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}

object? GetValue(object? value)
{
    if (value == null)
    {
        return null;
    }
    var element = (JsonElement)value;
    if (element.ValueKind == JsonValueKind.Null)
    {
        return null;
    }
    else if (element.ValueKind == JsonValueKind.True)
    {
        return true;
    }
    else if (element.ValueKind == JsonValueKind.False)
    {
        return false;
    }
    else if (element.ValueKind == JsonValueKind.False)
    {
        return false;
    }
    else if (element.ValueKind == JsonValueKind.Number)
    {
        return element.GetDouble();
    }
    else
    {
        return value.ToString();
    }
}
