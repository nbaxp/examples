using Wta.Infrastructure.Attributes;

namespace Wta.Infrastructure.Configuration;

[Options]
public class SmsOptions
{
    public string Url { get; set; } = default!;
    public string Key { get; set; } = default!;
    public string Secret { get; set; } = default!;
}
