using Flurl;
using HttpClientToCurl;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Distributed;

namespace Wta.Application.SystemModule.Controllers;

public class TokenController(
    ILogger<TokenController> logger,
    IRepository<LoginProvider> loginProviderRepository,
    TokenValidationParameters tokenValidationParameters,
    SigningCredentials signingCredentials,
    JwtSecurityTokenHandler jwtSecurityTokenHandler,
    JwtOptions jwtOptions,
    IEncryptionService passwordHasher,
    IStringLocalizer stringLocalizer,
    IRepository<User> userRepository,
    IRepository<UserLogin> userLoginRepository,
    IDistributedCache cache,
    IHttpClientFactory factory,
    IRepository<ExternalApp> repository) : BaseController
{
    /// <summary>
    /// 作为 OAuth2 服务器，接收来自浏览器的三方登录请求，跳转到登录页
    /// </summary>
    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [RawAction, HttpGet, AllowAnonymous, Ignore]
    public IActionResult Authorize(string client_id, string? state, string? redirect_uri, string response_type = "code")
    {
        var app = repository.AsNoTracking().FirstOrDefault(o => o.ClientId == client_id);
        if (app == null)
        {
            throw new ProblemException("应用不存在");
        }
        else if (app.Disabled)
        {
            throw new ProblemException("应用已禁用");
        }
        if (redirect_uri != null && !redirect_uri.StartsWith(new Url(app.Callback).Root))
        {
            throw new ProblemException("redirect_uri不匹配");
        }
        var return_to = Request.GetDisplayUrl();
        var anti_token = Guid.NewGuid().ToString("N");
        cache.Set(anti_token, anti_token);

        if (User.Identity != null && User.Identity.IsAuthenticated)//已登录，直接跳转到 callback
        {
            redirect_uri ??= app.Callback;
            var code = Guid.NewGuid().ToString("N");
            cache.Set(code, User.Identity.Name);
            var redirectUrl = redirect_uri.SetQueryParam("code", code).SetQueryParam("state", state).ToString();
            return Redirect(redirectUrl);
        }
        else//未登录，跳转到登录页
        {
            var url = this.Url.Content("~/")
                .SetQueryParam("client_name", app.Name)
                .SetQueryParam("client_id", client_id)
                .SetQueryParam("return_to", return_to)
                .SetQueryParam("anti_token", anti_token)
                .SetFragment("/login");
            return Redirect(url);
        }
    }

    /// <summary>
    /// 作为 OAuth2 服务器，接收三方登录的服务端请求，根据 code 提供 token
    /// </summary>
    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [RawAction, HttpPost, AllowAnonymous, Ignore]
    public IActionResult Token([FromForm] string client_id, [FromForm] string client_secret, [FromForm] string code, [FromForm] string? refresh_token, [FromForm] string grant_type = "authorization_code")
    {
        var app = repository.AsNoTracking().FirstOrDefault(o => o.ClientId == client_id);
        if (app == null)
        {
            return Problem("应用不存在");
        }
        else if (app.Disabled)
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
        if (grant_type == "authorization_code")//创建token
        {
        }
        else//刷新token
        {
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
        var access_token = this.CreateToken(subject, jwtOptions.AccessTokenExpires);
        var id_token = access_token;
        var refreshToken = this.CreateToken(subject, jwtOptions.RefreshTokenExpires);
        var expires_in = (long)jwtOptions.AccessTokenExpires.TotalSeconds;
        if (Request.Headers.Accept.Contains("application/json"))
        {
            return new JsonResult(new
            {
                token_type = "bearer",
                access_token,
                id_token,
                refresh_token = refreshToken,
                expires_in
            });
        }
        return Content($"token_type=bearer&access_token={access_token}&id_token={id_token}&refresh_token={refreshToken}&expires_in={expires_in}");
    }

    /// <summary>
    /// 作为 OAuth2 服务器，提供用户信息
    /// </summary>
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

    /// <summary>
    /// 作为 OAuth2 服务器，提供用户信息
    /// </summary>
    [ApiExplorerSettings(GroupName = "OAuth2 Server API"), Route("/api/oauth/[action]")]
    [RawAction, HttpGet, Authorize, Ignore]
    public async Task<IActionResult> Logout()
    {
        var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
        var user = userRepository
            .AsNoTracking()
            .Where(o => o.NormalizedUserName == normalizedUserName)
            .Include(o => o.UserLogins)
            .FirstOrDefault()!;
        foreach (var item in user.UserLogins)
        {
            var app = repository.AsNoTracking().FirstOrDefault(o => o.Name == item.LoginProvider);
            if (app?.Logout != null)
            {
                try
                {
                    await factory.CreateClient().PostAsync(app.Logout, null).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString());
                }
            }
        }
        return Ok();
    }

    /// <summary>
    /// 作为 OAuth2 客户端，接入的三方登录列表
    /// </summary>
    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]")]
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<List<object>> Providers(string provider, string returnUrl)
    {
        return Json(loginProviderRepository.AsNoTracking().Select(o => new { o.Name, o.Number, o.Icon } as object).ToList());
    }

    /// <summary>
    /// 作为 OAuth2 客户端，返回跳转到三方登录页的地址
    /// </summary>
    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]")]
    [HttpGet, AllowAnonymous, Ignore]
    public ApiResult<string> ExternalLogin(string provider, string returnUrl)
    {
        var loginProvider = loginProviderRepository
            .AsNoTracking()
            .First(o => o.Number == provider);
        var redirectUri = $"{Request.Scheme}{Uri.SchemeDelimiter}{Request.Host}{Request.PathBase}{Url.Content($"~{loginProvider.CallbackPath}")}";
        var url = loginProvider.AuthorizationEndpoint
            .SetQueryParam("client_id", loginProvider.ClientId)
            .SetQueryParam("redirect_uri", redirectUri)
            .ToString();
        var result = Json(url);
        result.IsRedirect = true;
        return result;
    }

    /// <summary>
    /// 作为 OAuth2 客户端，接受 OAuth2 Server 端的回调
    /// </summary>

    [ApiExplorerSettings(GroupName = "OAuth2 Client API"), Route("/api/oauth/[action]/{provider}")]
    [HttpGet, AllowAnonymous, Ignore]
    public async Task<IActionResult> OAuthCallback(string provider, string code)
    {
        var loginProvider = loginProviderRepository.AsNoTracking().First(o => o.Number == provider);
        //get token
        var tokenUrl = loginProvider.TokenEndpoint
            .SetQueryParam("client_id", loginProvider.ClientId)
            .SetQueryParam("client_secret", loginProvider.ClientSecret)
            .SetQueryParam("code", code)
            .SetQueryParam("redirect_uri", Url.Content($"~{loginProvider.CallbackPath}"));
        var client = factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, tokenUrl) { Content = new FormUrlEncodedContent(tokenUrl.Query.QueryStringToDictionary()) };
        _ = client.GenerateCurlInString(request);
        var response = await client.SendAsync(request).ConfigureAwait(false);
        var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var tokenResult = response.Content.Headers.ContentType?.MediaType == "application/json" ? result.JsonTextToDictionary() : result.QueryStringToDictionary();
        var access_token = tokenResult["access_token"];
        //get user id
        var userinfoUrl = loginProvider.UserInformationEndpoint;
        var client2 = factory.CreateClient();
        if (loginProvider.UserInformationTokenPosition == "header")
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", access_token);
        }
        else
        {
            userinfoUrl = userinfoUrl.SetQueryParam("access_token", access_token);
        }
        var response2 = loginProvider.UserInformationRequestMethod == "post" ? await client.PostAsync(tokenUrl.RemoveQuery(), new FormUrlEncodedContent(new Url(userinfoUrl).Query.QueryStringToDictionary())).ConfigureAwait(false) : await client.GetAsync(userinfoUrl).ConfigureAwait(false);
        var result2 = await response2.Content.ReadAsStringAsync().ConfigureAwait(false);
        var userInfoResult = response2.Content.Headers.ContentType?.MediaType == "application/json" ? result2.JsonTextToDictionary() : result2.QueryStringToDictionary();
        var openId = userInfoResult[loginProvider.UserIdName!];
        // 跳转到 SPA
        var url3 = Url.Content("~/")
            .SetQueryParam("provider", provider)
            .SetQueryParam("open_id", openId)
            .SetQueryParam("access_token",access_token)
            .SetFragment("/callback");
        return Redirect(url3);
        //var loginUser = userLoginRepository.AsNoTracking().Include(o => o.User).FirstOrDefault(o => o.LoginProvider == provider && o.ProviderKey == openId);
        //if (loginUser != null) // 已绑定用户，携带 token 跳转到浏览器，自动登录
        //{
        //    var user = loginUser.User;
        //    var additionalClaims = new List<Claim>();
        //    if (user.TenantNumber != null)
        //    {
        //        additionalClaims.Add(new Claim("TenantNumber", user.TenantNumber));
        //    }
        //    var normalizedUserName = user.NormalizedUserName;
        //    var roles = userRepository.AsNoTracking()
        //        .Where(o => o.NormalizedUserName == normalizedUserName)
        //        .SelectMany(o => o.UserRoles)
        //        .Select(o => o.Role!.Number)
        //        .ToList()
        //        .Select(o => new Claim(tokenValidationParameters.RoleClaimType, o!));
        //    additionalClaims.AddRange(roles);
        //    var subject = CreateSubject(user.UserName!, additionalClaims);
        //    var access_token = CreateToken(subject, jwtOptions.AccessTokenExpires);
        //    var refresh_token = CreateToken(subject, jwtOptions.RefreshTokenExpires);
        //    var url = Url.Content("~/").SetQueryParam("access_token", access_token).SetQueryParam("refresh_token", refresh_token).SetFragment("/oauth2-login");
        //    return Redirect(url);
        //}
        //else//未绑定用户，跳转到注册页注册并绑定
        //{
        //    var url3 = Url.Content("~/").SetQueryParam("provider", provider).SetQueryParam("open_id", openId).SetFragment("/callback");
        //    return Redirect(url3);
        //}
        //
        //if (User.Identity!.IsAuthenticated)// 已登录用户且未绑定，添加三方登录绑定
        //{
        //    if (!userLoginRepository.AsNoTracking().Any(o => o.LoginProvider == provider && o.ProviderKey == openId))// 没有 openid 增加 openid 并关联当前用户
        //    {
        //        var normalizedUserName = User.Identity?.Name?.ToUpperInvariant()!;
        //        var user = userRepository.Query().First(o => o.NormalizedUserName == normalizedUserName);
        //        user.UserLogins.Add(new UserLogin { LoginProvider = provider, ProviderKey = openId! });
        //        userRepository.SaveChanges();
        //    }
        //    return Ok();
        //}
        //else//未登录用户
        //{
        //}
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
            var tenantNumber = model.TenantNumber ?? "root";
            var user = userQuery.FirstOrDefault(o => o.NormalizedUserName == normalizedUserName && o.TenantNumber == tenantNumber);
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
                    if (!string.IsNullOrEmpty(model.provider))
                    {//oauth2 client
                        var userLogin = userLoginRepository.Query().FirstOrDefault(o => o.UserId == user.Id && o.LoginProvider == model.provider);
                        if (userLogin == null)
                        {
                            userLogin = new UserLogin
                            {
                                UserId = user.Id,
                                LoginProvider = model.provider,
                                ProviderKey = model.open_id!,
                                DisplayName = loginProviderRepository.AsNoTracking().FirstOrDefault(o => o.Number == model.provider)?.Name!
                            };
                            userLoginRepository.Add(userLogin);
                        }
                    }
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
            {//oauth2 server
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
                else if (app.Disabled)
                {
                    throw new ProblemException("应用已禁用");
                }
                var returnUrl = new Url(model.return_to);
                var redirect_uri = returnUrl.QueryParams.FirstOrDefault("redirect_uri").ToString();
                var state = returnUrl.QueryParams.FirstOrDefault("state")?.ToString();
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
