using Flurl;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Wta.Infrastructure.OAuth2;

namespace Wta.Application.SystemModule.Controllers;

public class TokenController(ILogger<TokenController> logger,
    OAuthService oauthService,
    TokenValidationParameters tokenValidationParameters,
    SigningCredentials signingCredentials,
    JwtSecurityTokenHandler jwtSecurityTokenHandler,
    JwtOptions jwtOptions,
    IEncryptionService passwordHasher,
    IStringLocalizer stringLocalizer,
    IRepository<User> userRepository,
    IRepository<UserLogin> userLoginRepository,
    IDistributedCache cache, IRepository<ExternalApp> repository) : BaseController
{
    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [HttpGet, AllowAnonymous, Ignore]
    public IActionResult Authorize(string client_id, string? state, string? redirect_uri)
    {
        var app = repository.AsNoTracking().FirstOrDefault(o => o.ClientId == client_id);
        if (app == null)
        {
            throw new ProblemException("应用不存在");
        }
        else if (!app.Enabled)
        {
            throw new ProblemException("应用已禁用");
        }
        if (redirect_uri != null && !redirect_uri.StartsWith(new Url(app.Callback).Root))
        {
            throw new ProblemException("redirect_uri不匹配");
        }
        var return_to = Request.GetDisplayUrl();
        //return RedirectToAction("Login", new { client_id, return_to });
        var anti_token = Guid.NewGuid().ToString("N");
        cache.Set(anti_token, anti_token);
        var url = this.Url.Content("~/")
            .SetQueryParam("client_name", app.Name)
            .SetQueryParam("client_id", client_id)
            .SetQueryParam("return_to", return_to)
            .SetQueryParam("anti_token", anti_token)
            .SetFragment("/login");
        return Redirect(url);
    }

    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [HttpPost, AllowAnonymous, Ignore]
    public IActionResult Token(string client_id, string client_secret, string code)
    {
        var app = repository.AsNoTracking().FirstOrDefault(o => o.ClientId == client_id);
        if (app == null)
        {
            return Problem("应用不存在");
        }
        else if (!app.Enabled)
        {
            return Problem("应用已禁用");
        }
        else if (app.ClientSecret != client_secret)
        {
            return Problem("client_secret无效");
        }
        var normalizedUserName = cache.Get<string>(code);
        if (string.IsNullOrEmpty(normalizedUserName))
        {
            return Problem("code已过期");
        }
        var user = userRepository.AsNoTracking().FirstOrDefault(o => o.NormalizedUserName == normalizedUserName);
        if (user == null)
        {
            return Problem("用户不存在");
        }
        else if (user.LockoutEnabled && user.LockoutEnd.HasValue && user.LockoutEnd.Value >= DateTime.UtcNow)
        {
            return Problem("用户已锁定");
        }
        var additionalClaims = new List<Claim>();
        if (user.TenantNumber != null)
        {
            additionalClaims.Add(new Claim("TenantNumber", user.TenantNumber));
        }
        var roles = userRepository.AsNoTracking()
            .Where(o => o.NormalizedUserName == normalizedUserName)
            .SelectMany(o => o.UserRoles)
            .Select(o => o.Role!.Number)
            .ToList()
            .Select(o => new Claim(tokenValidationParameters.RoleClaimType, o!));
        additionalClaims.AddRange(roles);
        var subject = CreateSubject(user.UserName!, additionalClaims);
        var token = this.CreateToken(subject, jwtOptions.AccessTokenExpires);
        return Content($"access_token={token}&token_type=bearer");
    }

    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [HttpGet, Authorize, Ignore]
    public IActionResult UserInfo()
    {
        var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
        var user = userRepository
            .AsNoTracking()
            .Where(o => o.NormalizedUserName == normalizedUserName).Select(o => new
            {
                o.Id,
                o.UserName,
                o.Avatar,
                o.Email,
                o.PhoneNumber,
                o.TenantNumber,
                Roles = o.UserRoles.Select(ur => ur.Role).Select(r => new { r!.Number, r!.Name })
            }).FirstOrDefault();
        return new JsonResult(user);
    }

    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]")]
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<string> ExternalLogin(string provider, string returnUrl)
    {
        var url = oauthService.GetAuthorizationUrl(provider);
        var result = Json(url);
        result.IsRedirect = true;
        return result;
    }

    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]")]
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<List<object>> Providers(string provider, string returnUrl)
    {
        return Json(oauthService.Options.Providers.Select(o => new { o.Name } as object).ToList());
    }

    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]/{provider}")]
    [HttpGet, AllowAnonymous, Ignore]
    public async Task<IActionResult> OAuthCallback(string provider, string code)
    {
        var openId = await oauthService.GetOpenId(provider, code).ConfigureAwait(false);
        if (User.Identity!.IsAuthenticated)// 已登录增加三方登录
        {
            if (!userLoginRepository.AsNoTracking().Any(o => o.LoginProvider == provider && o.ProviderKey == openId))// 没有 openid 增加 openid 并关联当前用户
            {
                var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
                var user = userRepository.Query().First(o => o.NormalizedUserName == normalizedUserName);
                user.UserLogins.Add(new UserLogin { LoginProvider = provider, ProviderKey = openId! });
                userRepository.SaveChanges();
            }
            return Ok();
        }
        else//未登录确认用户名并登录
        {
            var loginUser = userLoginRepository.AsNoTracking().Include(o => o.User).FirstOrDefault(o => o.LoginProvider == provider && o.ProviderKey == openId);
            if (loginUser == null) // 没有 openid 确认登录名
            {
                var url = Url.Content("~/").SetQueryParam("provider", provider).SetQueryParam("openId", openId).SetQueryParam("userName", $"{provider}_{openId}").SetFragment("/oauth2-register");
                return Redirect(url);
            }
            else
            {
                var user = loginUser.User;
                var additionalClaims = new List<Claim>();
                if (user.TenantNumber != null)
                {
                    additionalClaims.Add(new Claim("TenantNumber", user.TenantNumber));
                }
                var normalizedUserName = user.NormalizedUserName;
                var roles = userRepository.AsNoTracking()
                    .Where(o => o.NormalizedUserName == normalizedUserName)
                    .SelectMany(o => o.UserRoles)
                    .Select(o => o.Role!.Number)
                    .ToList()
                    .Select(o => new Claim(tokenValidationParameters.RoleClaimType, o!));
                additionalClaims.AddRange(roles);
                var subject = CreateSubject(user.UserName!, additionalClaims);
                var access_token = CreateToken(subject, jwtOptions.AccessTokenExpires);
                var refresh_token = CreateToken(subject, jwtOptions.RefreshTokenExpires);
                var url = Url.Content("~/").SetQueryParam("access_token", access_token).SetQueryParam("refresh_token", refresh_token).SetFragment("/oauth2-login");
                return Redirect(url);
            }
        }
    }

    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<object> Create()
    {
        return Json(typeof(LoginRequestModel).GetMetadataForType());
    }

    [HttpPost]
    [AllowAnonymous]
    public ApiResult<object> Create(LoginRequestModel model)
    {
        userRepository.DisableTenantFilter();
        if (ModelState.IsValid)
        {
            var userQuery = userRepository.Query();
            var normalizedUserName = model.UserName?.ToUpperInvariant()!;
            var user = userQuery.FirstOrDefault(o => o.NormalizedUserName == normalizedUserName && o.TenantNumber == model.TenantNumber);
            if (user != null)
            {
                if (user.LockoutEnd.HasValue)
                {
                    if (user.LockoutEnd.Value >= DateTime.UtcNow)
                    {
                        var minutes = GetLeftMinutes(user);
                        throw new ProblemException(string.Format(CultureInfo.InvariantCulture, "用户已锁定,{0}分钟后解除", minutes));
                    }
                    else
                    {
                        user.LockoutEnd = null;
                        user.AccessFailedCount = 0;
                    }
                }

                if (user.PasswordHash != passwordHasher.HashPassword(model.Password!, user.SecurityStamp!))
                {
                    user.AccessFailedCount++;
                    if (user.AccessFailedCount >= jwtOptions.MaxFailedAccessAttempts)
                    {
                        user.LockoutEnd = DateTime.UtcNow.Add(jwtOptions.DefaultLockout);
                        user.AccessFailedCount = 0;
                        userRepository.SaveChanges();
                        var minutes = GetLeftMinutes(user);
                        throw new ProblemException(string.Format(CultureInfo.InvariantCulture, "用户已锁定,{0}分钟后解除", minutes));
                    }
                    else
                    {
                        userRepository.SaveChanges();
                        throw new ProblemException($"密码错误,剩余尝试错误次数为 {jwtOptions.MaxFailedAccessAttempts - user.AccessFailedCount}");
                    }
                }
                else
                {
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;
                    userRepository.SaveChanges();
                }
            }
            else
            {
                throw new ProblemException(stringLocalizer.GetString("用户名或密码错误"));
            }
            //
            var additionalClaims = new List<Claim>();
            if (user.TenantNumber != null)
            {
                additionalClaims.Add(new Claim("TenantNumber", user.TenantNumber));
            }
            var roles = userRepository.AsNoTracking()
                .Where(o => o.NormalizedUserName == normalizedUserName)
                .SelectMany(o => o.UserRoles)
                .Select(o => o.Role!.Number)
                .ToList()
                .Select(o => new Claim(tokenValidationParameters.RoleClaimType, o!));
            additionalClaims.AddRange(roles);
            var subject = CreateSubject(model.UserName!, additionalClaims);
            var result = new LoginResponseModel
            {
                AccessToken = CreateToken(subject, jwtOptions.AccessTokenExpires),
                RefreshToken = CreateToken(subject, model.RememberMe ? TimeSpan.FromDays(365) : jwtOptions.RefreshTokenExpires),
                ExpiresIn = (long)jwtOptions.AccessTokenExpires.TotalSeconds
            };
            if (!string.IsNullOrEmpty(model.client_id))
            {//oauth2
                if (model.anti_token == null)
                {
                    throw new ProblemException("anti_token不能为空");
                }
                var antiToken = cache.Get<string>(model.anti_token);
                if (antiToken == null)
                {
                    throw new ProblemException("anti_token已过期");
                }
                var app = repository.AsNoTracking().FirstOrDefault(o => o.ClientId == model.client_id);
                if (app == null)
                {
                    throw new ProblemException("应用不存在");
                }
                else if (!app.Enabled)
                {
                    throw new ProblemException("应用已禁用");
                }
                var returnUrl = new Url(model.return_to);
                var redirect_uri = returnUrl.QueryParams.FirstOrDefault("redirect_uri")?.ToString();
                var state = returnUrl.QueryParams.FirstOrDefault("state");
                if (redirect_uri != null && !redirect_uri.StartsWith(new Url(app.Callback).Root))
                {
                    throw new ProblemException("redirect_uri不匹配");
                }
                redirect_uri ??= app.Callback;
                var code = Guid.NewGuid().ToString("N");
                cache.Set(code, user.NormalizedUserName);
                var url = redirect_uri.SetQueryParam("code", code).SetQueryParam("state", state).ToString();
                var oauthResult = Json(url as object);
                oauthResult.IsRedirect = true;
                return oauthResult;
            }
            return Json(result as object);
        }
        throw new BadRequestException();
    }

    [HttpPost]
    [AllowAnonymous]
    public ApiResult<LoginResponseModel> Refresh([FromBody] string refreshToken)
    {
        var validationResult = jwtSecurityTokenHandler.ValidateTokenAsync(refreshToken, tokenValidationParameters).Result;
        if (!validationResult.IsValid)
        {
            throw new ProblemException("RefreshToken验证失败", innerException: validationResult.Exception);
        }
        var subject = validationResult.ClaimsIdentity;
        var result = new LoginResponseModel
        {
            AccessToken = CreateToken(subject, jwtOptions.AccessTokenExpires),
            RefreshToken = CreateToken(subject, validationResult.SecurityToken.ValidTo.Subtract(validationResult.SecurityToken.ValidFrom)),
            ExpiresIn = (long)jwtOptions.AccessTokenExpires.TotalSeconds
        };
        return Json(result);
    }

    private ClaimsIdentity CreateSubject(string userName, List<Claim> additionalClaims)
    {
        var claims = new List<Claim>(additionalClaims) { new(tokenValidationParameters.NameClaimType, userName) };
        var subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return subject;
    }

    private string CreateToken(ClaimsIdentity subject, TimeSpan timeout)
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

    private static string GetLeftMinutes(User user)
    {
        return (user.LockoutEnd!.Value - DateTime.UtcNow).TotalMinutes.ToString("f1", CultureInfo.InvariantCulture);
    }
}
