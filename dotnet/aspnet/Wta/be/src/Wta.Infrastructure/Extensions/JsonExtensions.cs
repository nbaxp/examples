namespace Wta.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string? ToJson(this object? obj, JsonSerializerOptions? options = null)
    {
        if (obj == null)
        {
            return null;
        }
        return JsonSerializer.Serialize(obj, options ?? GetJsonOptions());
    }

    public static T? FromJson<T>(this string? value, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value, options ?? GetJsonOptions());
    }

    private static JsonSerializerOptions GetJsonOptions()
    {
        var options = Global.Application.Services.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
        return options;
    }
}
