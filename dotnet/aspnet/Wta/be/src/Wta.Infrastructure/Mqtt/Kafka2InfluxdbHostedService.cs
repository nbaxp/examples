//using Confluent.Kafka;
//using InfluxDB.Client;
//using InfluxDB.Client.Writes;

//namespace Wta.Infrastructure.Mqtt;

//[Service<IHostedService>]
//public class Kafka2InfluxdbHostedService(IHostEnvironment env, IConfiguration configuration) : IHostedService
//{
//    public IConsumer<Ignore, string> KafkaConsumer { get; private set; } = default!;

//    public Task StartAsync(CancellationToken cancellationToken)
//    {
//        if (env.IsDevelopment())
//        {
//            var connectionString = configuration.GetConnectionString("Kafka")!;
//            var builder = new UriBuilder(connectionString);
//            var protocol = Enum.Parse<SecurityProtocol>(builder.Scheme, true);
//            var config = new ConsumerConfig
//            {
//                ClientId = "kafka4test",
//                GroupId = "kafka4test",
//                EnableAutoCommit = false,
//                BootstrapServers = $"{builder.Host}:{builder.Port}",
//                SecurityProtocol = protocol,
//                AllowAutoCreateTopics = true,
//                StatisticsIntervalMs = 5 * 1000,
//                ReconnectBackoffMs = 5 * 1000,
//                ReconnectBackoffMaxMs = 3600 * 1000,
//                Acks = Acks.All,
//            };
//            KafkaConsumer = new ConsumerBuilder<Ignore, string>(config).Build();
//            KafkaConsumer.Subscribe("^topic_");
//            _ = Task.Run(() =>
//            {
//                while (!cancellationToken.IsCancellationRequested)
//                {
//                    try
//                    {
//                        var consumeResult = KafkaConsumer.Consume(cancellationToken);
//                        if (consumeResult.IsPartitionEOF)
//                        {
//                            Console.WriteLine(
//                                $"Reached end of topic {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}.");

//                            continue;
//                        }
//                        Console.WriteLine($"kafka: {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
//                        using var client = new InfluxDBClient("http://localhost:8086",
//                            "admin",
//                            "aA123456",
//                            "wta",
//                            "autogen");
//                        using var api = client.GetWriteApi();
//                        var point = PointData.Measurement(consumeResult.Topic)
//                        .Tag("topic", consumeResult.Topic);
//                        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(consumeResult.Message.Value);
//                        dict?.ForEach(kv =>
//                        {
//                            point.Field(kv.Key, kv.Value);
//                        });
//                        api.WritePoint(point);
//                        KafkaConsumer.Commit(consumeResult);
//                    }
//                    catch (KafkaException e)
//                    {
//                        Console.WriteLine($"Store Offset error: {e.Error.Reason}");
//                    }
//                }
//            }, cancellationToken);
//        }
//        return Task.CompletedTask;
//    }

//    public Task StopAsync(CancellationToken cancellationToken)
//    {
//        if (env.IsDevelopment())
//        {
//            try
//            {
//                KafkaConsumer.Dispose();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Kafka 客户端释放失败");
//                Console.WriteLine(ex.Message);
//                Console.WriteLine(ex.ToString());
//            }
//        }
//        return Task.CompletedTask;
//    }
//}
