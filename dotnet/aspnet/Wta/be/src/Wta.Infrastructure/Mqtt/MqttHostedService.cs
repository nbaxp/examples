using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace Wta.Infrastructure.Mqtt;

[Service<IHostedService>]
public class MqttHostedService(IConfiguration configuration) : IHostedService
{
    public IManagedMqttClient Client { get; private set; } = default!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var connectionString = configuration.GetConnectionString("Mqtt")!;
        var builder = new UriBuilder(connectionString);
        var options = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId("wta.platform")
                .WithTcpServer(builder.Host, builder.Port)
                .WithCredentials(builder.UserName, Encoding.UTF8.GetBytes(builder.Password))
                .WithCleanSession(false)
                .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithSessionExpiryInterval(uint.MaxValue))
            .Build();
        Client = new MqttFactory().CreateManagedMqttClient();
        Client.ApplicationMessageReceivedAsync += e =>
        {
            try
            {
                Console.WriteLine("Received application message.");
                var message = e.ApplicationMessage.ConvertPayloadToString();
                Console.WriteLine(message);
                //e.ReasonCode = MqttApplicationMessageReceivedReasonCode.Success;
                //e.ResponseReasonString = e.ReasonCode.ToString();
                //e.IsHandled = true;
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(message)!;
                if (int.TryParse(dict["msg"].ToString(), out var result) && result % 2 == 0)
                {
                    throw new Exception("message need resent");
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                e.ProcessingFailed = true;
                return Task.FromException(ex);
            }
        };
        await Client.SubscribeAsync("topic/#", MqttQualityOfServiceLevel.ExactlyOnce).ConfigureAwait(false);
        await Client.StartAsync(options).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            Client?.StopAsync();
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
}
