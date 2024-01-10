using System.Text.Json.Serialization;

namespace Wta.Application.Identity.Models;

public class LoginResponseModel
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
}
