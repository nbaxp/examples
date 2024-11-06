using MQTTnet.AspNetCore;

namespace Wta.MqttServer;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(b =>
            {
                b.UseKestrel(o =>
                {
                    o.ListenAnyIP(1883, p => p.UseMqtt());
                    o.ListenAnyIP(5005);
                });
                b.UseStartup<Startup>();
            })
            .RunConsoleAsync();
    }
}
