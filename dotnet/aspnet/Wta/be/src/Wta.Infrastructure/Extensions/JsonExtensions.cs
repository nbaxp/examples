namespace Wta.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string? ToJson(this object? obj)
    {
        if (obj == null)
        {
            return null;
        }
        return JsonSerializer.Serialize(obj, GetJsonOptions());
    }

    public static T? FromJson<T>(this string? value)
    {
        if (value == null)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value, GetJsonOptions());
    }

    private static JsonSerializerOptions GetJsonOptions()
    {
        return WtaApplication.Application.Services.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
    }
}
