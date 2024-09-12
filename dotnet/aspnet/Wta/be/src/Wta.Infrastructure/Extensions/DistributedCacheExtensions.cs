using Microsoft.Extensions.Caching.Distributed;

namespace Wta.Infrastructure.Extensions;

public static class DistributedCacheExtensions
{
    public static T? Get<T>(this IDistributedCache cache, string key)
    {
        return Encoding.UTF8.GetString(cache.Get(key) ?? []).FromJson<T>();
    }

    public static void Set<T>(this IDistributedCache cache, string key, T? value, int expiration = 600)
    {
        cache.Set(key, Encoding.UTF8.GetBytes(value.ToJson() ?? string.Empty), new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(expiration) });
    }
}
