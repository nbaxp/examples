using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http.Headers;
using System.Text.Json;

namespace OAuth2WebClient;

public static class OAuthExtensions
{
    public static AuthenticationBuilder AddOAuth2(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<OAuthOptions> configureOptions, string sub = "id")
    {
        return builder.AddOAuth<OAuthOptions, OAuthHandler<OAuthOptions>>(authenticationScheme, displayName, o =>
        {
            //o.ClientId = "aspnetcore";
            //o.ClientSecret = "123456";
            //o.AuthorizationEndpoint = "http://localhost:5000/api/oauth/authorize";
            //o.TokenEndpoint = "http://localhost:5000/api/oauth/token";
            //o.UserInformationEndpoint = "http://localhost:5000/api/oauth/user-info";
            o.CallbackPath = "/oauth-callback";
            o.ClaimActions.MapJsonKey("sub", sub);
            o.Events.OnCreatingTicket = async context =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                response.EnsureSuccessStatusCode();
                using var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                context.RunClaimActions(user.RootElement);
            };
            configureOptions.Invoke(o);
        });
    }
}