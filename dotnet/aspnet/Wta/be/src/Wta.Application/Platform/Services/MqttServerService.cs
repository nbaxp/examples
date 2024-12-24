using MQTTnet;
using Vibrant.InfluxDB.Client.Rows;
using Vibrant.InfluxDB.Client;
using Wta.Infrastructure.Mqtt;
using System.Text.Json;

namespace Wta.Application.Platform.Services;

[Service<IMqttServerService>]
public class MqttServerService(IEncryptionService encryptionService, IRepository<User> userRepository) : IMqttServerService
{
    public bool Valid(string clientId, string userName, string password)
    {
        userRepository.DisableTenantFilter();
        var tenantNumber = clientId.Split('.').First();
        var user = userRepository
            .AsNoTracking()
            .FirstOrDefault(o => o.TenantNumber == tenantNumber && o.UserName == userName);
        if (user != null && user.PasswordHash == encryptionService.HashPassword(password, user.SecurityStamp))
        {
            return true;
        }
        return false;
    }

    public async void Receive(string clientId, MqttApplicationMessage applicationMessage)
    {
        try
        {
            var payloadText = string.Empty;
            if (applicationMessage.Payload.Length > 0)
            {
                payloadText = Encoding.UTF8.GetString(applicationMessage.Payload).Trim();
                payloadText = Regex.Replace(payloadText, @"\s+", " ");
            }
            Console.WriteLine($"publish: '{clientId}' => {payloadText}");
            using var client = new InfluxClient(new Uri("http://localhost:8086"), "admin", "aA123456");
            var dbName = "wta";
            var measurementName = "device";
            await client.CreateDatabaseAsync(dbName).ConfigureAwait(false);
            var row = new DynamicInfluxRow
            {
                Timestamp = DateTime.UtcNow
            };
            row.SetTag("tenant", clientId.Split('.').First());
            row.SetTag("client", clientId);
            row.SetField("topic", applicationMessage.Topic);
            row.SetField("message", payloadText);
            var paths = applicationMessage.Topic.Split('/');
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

    private static object? GetValue(object? value)
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
}
