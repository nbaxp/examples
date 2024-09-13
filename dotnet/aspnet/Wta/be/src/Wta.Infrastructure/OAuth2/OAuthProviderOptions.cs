namespace Wta.Infrastructure.OAuth2;

public class OAuthProviderOptions
{
    public string Name { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string? ClientIdName { get; set; }
    public string? ClientSecretName { get; set; }
    public string? UserIdName { get; set; }
    public string CallbackPath { get; set; } = null!;
    public string AuthorizationEndpoint { get; set; } = null!;
    public string TokenEndpoint { get; set; } = null!;
    public string TokenRequestMethod { get; set; } = null!;
    public string TokenRequestPayload { get; set; } = null!;
    public string TokenResponseType { get; set; } = null!;
    public string UserInformationEndpoint { get; set; } = null!;
    public string UserInformationRequestMethod { get; set; } = null!;
    public string UserInformationRequestPayload { get; set; } = null!;
    public string UserInformationResponseType { get; set; } = null!;
    public string? UserIdentificationEndpoint { get; set; }
}
