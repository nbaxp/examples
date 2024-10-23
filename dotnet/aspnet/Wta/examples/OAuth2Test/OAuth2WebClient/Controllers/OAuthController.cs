using Microsoft.AspNetCore.Mvc;

namespace OAuth2WebClient.Controllers;

public class OAuthController:Controller
{
    [Route("/oauth/callback/openiddict")]
    public IActionResult Callback()
    {
        return null;
    }
}
