
namespace Wta.MqttServer;

public static class Extensions
{
    public static string ToEscapeString(this string input)
    {
        return input;
        return input.Replace("\\", "\\\\")
                .Replace("\"", "\\\"");
    }
}
