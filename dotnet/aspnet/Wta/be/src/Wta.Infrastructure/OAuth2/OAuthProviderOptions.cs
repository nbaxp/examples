namespace Wta.Infrastructure.OAuth2;

public class OAuthProviderOptions
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string? UserIdName { get; set; }
    public string CallbackPath { get; set; } = null!;
    public string AuthorizationEndpoint { get; set; } = null!;
    public string TokenEndpoint { get; set; } = null!;
    public string UserInformationEndpoint { get; set; } = null!;
    public string UserInformationRequestMethod { get; set; } = null!;
    public string UserInformationTokenPosition { get; set; } = null!;
}
