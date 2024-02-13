using Wta.Infrastructure.Attributes;

namespace Wta.Infrastructure.Configuration;

[Options]
public class AppOptions
{
    public bool UseRedis { get; set; }
}
