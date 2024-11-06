using MQTTnet;
using MQTTnet.Protocol;

namespace Wta.Infrastructure.Mqtt;

[Service<IHostedService>]
public class MqttHostedService(IConfiguration configuration) : IHostedService
{
    public IMqttClient Client { get; private set; } = default!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("Mqtt")!;
        var builder = new UriBuilder(connectionString);
        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId("wta.platform")
            .WithTcpServer(builder.Host, builder.Port)
            .WithCredentials(builder.UserName, Encoding.UTF8.GetBytes(builder.Password))
            .WithCleanSession(false)
            .WithSessionExpiryInterval(uint.MaxValue)
            .Build();
        var mqttFactory = new MqttClientFactory();
        Client = mqttFactory.CreateMqttClient();
        Client.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine("Received application message.");
            var message = e.ApplicationMessage.ConvertPayloadToString();
            //e.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;
            //e.ResponseReasonString = e.ReasonCode.ToString();
            e.IsHandled = true;
            Console.WriteLine(message);
            return Task.CompletedTask;
        };
        await Connect(mqttClientOptions).ConfigureAwait(false);
        _ = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (!await Client.TryPingAsync().ConfigureAwait(false))
                    {
                        await Connect(mqttClientOptions).ConfigureAwait(false);
                    }
                    else
                    {
                        Console.WriteLine("MQTT 客户端连接可用");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("MQTT 客户端连接失败");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.ToString());
                }
                await Task.Delay(5 * 1000).ConfigureAwait(false);
            }
        }, cancellationToken).ConfigureAwait(false);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            Client?.Dispose();
        }
        catch (Exception ex)
        {
            Console.WriteLine("MQTT 客户端释放失败");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.ToString());
        }
        return Task.CompletedTask;
    }

    private async Task Connect(MqttClientOptions mqttClientOptions)
    {
        try
        {
            await Client.ConnectAsync(mqttClientOptions, CancellationToken.None).ConfigureAwait(false);
            await Client.SubscribeAsync("topic/#", MqttQualityOfServiceLevel.ExactlyOnce).ConfigureAwait(false);
            Console.WriteLine("MQTT 客户端连接成功");
        }
        catch (Exception ex)
        {
            Console.WriteLine("MQTT 客户端连接失败");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.ToString());
        }
    }
}
