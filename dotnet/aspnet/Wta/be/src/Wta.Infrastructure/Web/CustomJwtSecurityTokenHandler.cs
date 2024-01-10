using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Wta.Infrastructure.Web;

public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
{
    private readonly IServiceProvider _serviceProvider;

    public CustomJwtSecurityTokenHandler(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        var jwtHandler = this._serviceProvider.GetRequiredService<JsonWebTokenHandler>();
        var validationResult = jwtHandler.ValidateTokenAsync(token, validationParameters).Result;
        if (validationResult.IsValid)
        {
            validatedToken = validationResult.SecurityToken;
            return new CustomClaimsPrincipal(this._serviceProvider, new ClaimsPrincipal(validationResult.ClaimsIdentity));
        }
        throw validationResult.Exception;
    }
}
