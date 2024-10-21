using Flurl;
using HttpClientToCurl;
using Microsoft.AspNetCore.Http;

namespace Wta.Infrastructure.OAuth2;

public class OAuthService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor, IOptions<OAuthOptions> options)
{
    private const string CLIENT_ID_NAME = "client_id";
    private const string CLIENT_SECRET_NAME = "client_secret";
    private const string REDIRECT_URI = "redirect_uri";
    private const string CODE_NAME = "code";
    private const string ACCESS_TOKEN_NAME = "access_token";

    public OAuthOptions Options { get; private set; } = options.Value;

    public string GetAuthorizationUrl(string provider)
    {
        var options = this.Options.Providers.First(o => o.Name == provider);
        var url = options.AuthorizationEndpoint
            .SetQueryParam(CLIENT_ID_NAME, options.ClientId)
            .SetQueryParamIf(options.AuthorizationEndpoint.Contains(REDIRECT_URI), REDIRECT_URI, UrlContent(options.CallbackPath));
        return url;
    }

    public async Task<string?> GetOpenId(string provider, string code)
    {
        var options = this.Options.Providers.First(o => o.Name == provider);
        var tokenResult = await GetToken(options, code).ConfigureAwait(false);
        var userInfoResult = await GetUserInfoInternal(options, tokenResult[ACCESS_TOKEN_NAME]).ConfigureAwait(false);
        if (userInfoResult.TryGetValue(options.UserIdName!, out var userId))
        {
            return userId;
        }
        return null;
    }

    public async Task<Dictionary<string, string>> GetToken(OAuthProviderOptions options, string code)
    {
        var url = options.TokenEndpoint
            .SetQueryParam(CLIENT_ID_NAME, options.ClientId)
            .SetQueryParam(CLIENT_SECRET_NAME, options.ClientSecret)
            .SetQueryParam(CODE_NAME, code)
            .SetQueryParamIf(options.TokenEndpoint.Contains(REDIRECT_URI), REDIRECT_URI, UrlContent(options.CallbackPath));
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(url.Query.QueryStringToDictionary()) };
        _ = client.GenerateCurlInString(request);
        var response = await client.SendAsync(request).ConfigureAwait(false);
        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return response.Content.Headers.ContentType?.MediaType == "application/json" ? result.JsonTextToDictionary() : result.QueryStringToDictionary();
    }

    public async Task<Dictionary<string, string>> GetUserInfoInternal(OAuthProviderOptions options, string access_token)
    {
        var url = options.UserInformationEndpoint;
        var client = factory.CreateClient();
        if (options.UserInformationTokenPosition == "header")
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
        }
        else
        {
            url = url.SetQueryParam(ACCESS_TOKEN_NAME, access_token);
        }
        var response = options.UserInformationRequestMethod == "post" ? await client.PostAsync(url.RemoveQuery(), new FormUrlEncodedContent(new Url(url).Query.QueryStringToDictionary())).ConfigureAwait(false) : await client.GetAsync(url).ConfigureAwait(false);
        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return response.Content.Headers.ContentType?.MediaType == "application/json" ? result.JsonTextToDictionary() : result.QueryStringToDictionary();
    }

    private string UrlContent(string targetPath)
    {
        if (string.IsNullOrEmpty(targetPath))
        {
            return targetPath;
        }

        if (!targetPath.StartsWith("/", StringComparison.Ordinal))
        {
            return targetPath;
        }
        var request = httpContextAccessor.HttpContext!.Request;
        var url = $"{request.Scheme}{Uri.SchemeDelimiter}{request.Host}{request.PathBase}{targetPath}";
        return url;
    }
}
