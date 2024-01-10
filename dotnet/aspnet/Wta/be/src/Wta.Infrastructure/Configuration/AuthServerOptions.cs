using Wta.Infrastructure.Attributes;

namespace Wta.Infrastructure.Configuration;

[Options]
public class AuthServerOptions
{
    public string Url { get; set; } = "";
}
