using Confluent.Kafka;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace Wta.Infrastructure.Mqtt;

[Service<IHostedService>]
public class Mqtt2KafkaHostedService(IConfiguration configuration) : IHostedService
{
    public IManagedMqttClient MqttClient { get; private set; } = default!;

    public IProducer<Null, string> KafkaProducer { get; private set; } = default!;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        StartKafka(configuration);
        await StartMqttAsync(configuration).ConfigureAwait(false);
    }

    private void StartKafka(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Kafka")!;
        var builder = new UriBuilder(connectionString);
        var protocol = Enum.Parse<SecurityProtocol>(builder.Scheme, true);
        var config = new ProducerConfig
        {
            ClientId = "mqtt2kafka",
            BootstrapServers = $"{builder.Host}:{builder.Port}",
            SecurityProtocol = protocol,
            AllowAutoCreateTopics = true,
            StatisticsIntervalMs = 5 * 1000,
            ReconnectBackoffMs = 5 * 1000,
            ReconnectBackoffMaxMs = 3600 * 1000,
            EnableDeliveryReports = true,
            Acks = Acks.All,
        };
        KafkaProducer = new ProducerBuilder<Null, string>(config)
            .SetLogHandler((_, message) =>
            {
                Console.WriteLine($"Facility: {message.Facility}-{message.Level} Message: {message.Message}");
            })
            .Build();
    }

    private async Task StartMqttAsync(IConfiguration configuration)
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
        MqttClient = new MqttFactory().CreateManagedMqttClient();
        MqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                Console.WriteLine("Received application message.");
                var message = e.ApplicationMessage.ConvertPayloadToString();
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(message)!;
                dict["topic"] = e.ApplicationMessage.Topic;
                Console.WriteLine(message);
                var topic = e.ApplicationMessage.Topic.Replace("/", "_");
                var result = await KafkaProducer.ProduceAsync(topic,
                    new Message<Null, string>
                    {
                        Value = JsonSerializer.Serialize(dict)
                    }).ConfigureAwait(false);
                Console.WriteLine(result.TopicPartitionOffset);
            }
            catch (Exception ex)
            {
                e.ProcessingFailed = true;
                Console.WriteLine(ex);
                await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            }
        };
        await MqttClient.SubscribeAsync("topic/#", MqttQualityOfServiceLevel.ExactlyOnce).ConfigureAwait(false);
        await MqttClient.StartAsync(options).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            MqttClient?.StopAsync();
            MqttClient?.Dispose();
            KafkaProducer?.Dispose();
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
