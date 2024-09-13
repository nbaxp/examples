namespace Wta.Infrastructure.OAuth2;

[Options]
public class OAuthOptions
{
    public List<OAuthProviderOptions> Providers { get; set; } = new List<OAuthProviderOptions>();
}
