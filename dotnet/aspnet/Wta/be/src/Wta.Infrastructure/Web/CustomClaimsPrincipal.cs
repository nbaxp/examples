using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Wta.Infrastructure.Configuration;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.Interfaces;

namespace Wta.Infrastructure.Web;

public class CustomClaimsPrincipal(IServiceProvider serviceProvider, ClaimsPrincipal claimsPrincipal) : ClaimsPrincipal(claimsPrincipal)
{
    public override bool IsInRole(string role)
    {
        using var scope = serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetService<IAuthService>();
        var config = scope.ServiceProvider.GetRequiredService<IOptions<AuthServerOptions>>().Value;
        var url = config.Url;
        if (authService != null)
        {
            return authService.HasPermission(role);
        }
        else
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ProblemException("AuthServer:Url is empty");
            }
            var requestUrl = $"{url.TrimEnd('/')}/api/user/has-permission";
            var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var client = httpClientFactory.CreateClient();
            var key = "Authorization";
            client.DefaultRequestHeaders.Remove(key);
            client.DefaultRequestHeaders.Add(key, httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString());
            var response = client.PostAsync(requestUrl, JsonContent.Create(role)).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = JsonSerializer.Deserialize<CustomApiResponse<bool>>(response.Content.ReadAsStringAsync().Result);
                return apiResponse!.Data;
            }
            return false;
        }
    }
}
