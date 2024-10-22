namespace Wta.Application.SystemModule.Services;

[Service<TokenService>]
public class TokenService(TokenValidationParameters tokenValidationParameters,
    SigningCredentials signingCredentials,
    JwtSecurityTokenHandler jwtSecurityTokenHandler)
{
    public string CreateToken(ClaimsIdentity subject, TimeSpan timeout)
    {
        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            // 签发者
            Issuer = tokenValidationParameters.ValidIssuer,
            // 接收方
            Audience = tokenValidationParameters.ValidAudience,
            // 凭据
            SigningCredentials = signingCredentials,
            // 声明
            Subject = subject,
            // 签发时间
            IssuedAt = now,
            // 生效时间
            NotBefore = now,
            // UTC 过期时间
            Expires = now.Add(timeout),
        };
        var securityToken = jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        return token;
    }

    public ClaimsIdentity CreateSubject(string userName, List<Claim> additionalClaims)
    {
        var claims = new List<Claim>(additionalClaims) { new(tokenValidationParameters.NameClaimType, userName) };
        var subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return subject;
    }
}
