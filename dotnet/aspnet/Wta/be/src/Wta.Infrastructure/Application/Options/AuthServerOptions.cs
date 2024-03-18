using Wta.Infrastructure.Attributes;

namespace Wta.Infrastructure.Application.Configuration;

[Options]
public class AuthServerOptions
{
    public string Url { get; set; } = "";
}
