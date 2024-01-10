using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Wta.Application.Identity.Domain;
using Wta.Application.Identity.Models;
using Wta.Infrastructure.Configuration;
using Wta.Infrastructure.Controllers;
using Wta.Infrastructure.Exceptions;
using Wta.Infrastructure.Interfaces;
using Wta.Infrastructure.Web;

namespace Wta.Application.Identity.Controllers;

public class TokenController : BaseController
{
    private readonly ILogger<TokenController> _loger;
    private readonly TokenValidationParameters _tokenValidationParameters;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly JwtOptions _jwtOptions;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IStringLocalizer _stringLocalizer;
    private readonly IRepository<User> _userRepository;

    public TokenController(ILogger<TokenController> logger,
        TokenValidationParameters tokenValidationParameters,
        SigningCredentials signingCredentials,
        JwtSecurityTokenHandler jwtSecurityTokenHandler,
        JwtOptions jwtOptions,
        IPasswordHasher passwordHasher,
        IStringLocalizer stringLocalizer,
        IRepository<User> userRepository)
    {
        _loger = logger;
        _tokenValidationParameters = tokenValidationParameters;
        _signingCredentials = signingCredentials;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        _jwtOptions = jwtOptions;
        _passwordHasher = passwordHasher;
        _stringLocalizer = stringLocalizer;
        _userRepository = userRepository;
    }

    [HttpPost]
    [AllowAnonymous]
    public CustomApiResponse<LoginResponseModel> Create(LoginRequestModel model)
    {
        if (ModelState.IsValid)
        {
            var additionalClaims = new List<Claim>();
            var userQuery = _userRepository.Query();
            var user = userQuery.FirstOrDefault(o => o.UserName == model.UserName);
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

                if (user.PasswordHash != _passwordHasher.HashPassword(model.Password!, user.SecurityStamp!))
                {
                    user.AccessFailedCount++;
                    if (user.AccessFailedCount >= _jwtOptions.MaxFailedAccessAttempts)
                    {
                        user.LockoutEnd = DateTime.UtcNow.Add(_jwtOptions.DefaultLockout);
                        user.AccessFailedCount = 0;
                        _userRepository.SaveChanges();
                        var minutes = GetLeftMinutes(user);
                        throw new ProblemException(string.Format(CultureInfo.InvariantCulture, "用户已锁定,{0}分钟后解除", minutes));
                    }
                    else
                    {
                        _userRepository.SaveChanges();
                        throw new ProblemException($"密码错误,剩余尝试错误次数为 {_jwtOptions.MaxFailedAccessAttempts - user.AccessFailedCount}");
                    }
                }
                else
                {
                    user.LockoutEnd = null;
                    user.AccessFailedCount = 0;
                    _userRepository.SaveChanges();
                }
            }
            else
            {
                throw new ProblemException(_stringLocalizer.GetString("用户名或密码错误"));
            }
            //
            var roles = _userRepository.AsNoTracking()
                .Where(o => o.UserName == model.UserName)
                .SelectMany(o => o.UserRoles)
                .Select(o => o.Role!.Number)
                .ToList()
                .Select(o => new Claim(_tokenValidationParameters.RoleClaimType, o!));
            additionalClaims.AddRange(roles);
            var subject = CreateSubject(model.UserName!, additionalClaims);
            var result = new LoginResponseModel
            {
                AccessToken = CreateToken(subject, _jwtOptions.AccessTokenExpires),
                RefreshToken = CreateToken(subject, model.RememberMe ? TimeSpan.FromDays(365) : _jwtOptions.RefreshTokenExpires),
                ExpiresIn = (long)_jwtOptions.AccessTokenExpires.TotalSeconds
            };
            return Json(result);
        }
        throw new BadRequestException();
    }

    [HttpPost]
    [AllowAnonymous]
    public CustomApiResponse<LoginResponseModel> Refresh([FromBody] string refreshToken)
    {
        var validationResult = _jwtSecurityTokenHandler.ValidateTokenAsync(refreshToken, _tokenValidationParameters).Result;
        if (!validationResult.IsValid)
        {
            throw new ProblemException("RefreshToken验证失败", innerException: validationResult.Exception);
        }
        var subject = validationResult.ClaimsIdentity;
        var result = new LoginResponseModel
        {
            AccessToken = CreateToken(subject, _jwtOptions.AccessTokenExpires),
            RefreshToken = CreateToken(subject, validationResult.SecurityToken.ValidTo.Subtract(validationResult.SecurityToken.ValidFrom)),
            ExpiresIn = (long)_jwtOptions.AccessTokenExpires.TotalSeconds
        };
        return Json(result);
    }

    private ClaimsIdentity CreateSubject(string userName, List<Claim> additionalClaims)
    {
        var claims = new List<Claim>(additionalClaims) { new Claim(_tokenValidationParameters.NameClaimType, userName) };
        var subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return subject;
    }

    private string CreateToken(ClaimsIdentity subject, TimeSpan timeout)
    {
        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            // 签发者
            Issuer = _tokenValidationParameters.ValidIssuer,
            // 接收方
            Audience = _tokenValidationParameters.ValidAudience,
            // 凭据
            SigningCredentials = _signingCredentials,
            // 声明
            Subject = subject,
            // 签发时间
            IssuedAt = now,
            // 生效时间
            NotBefore = now,
            // UTC 过期时间
            Expires = now.Add(timeout),
        };
        var securityToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var token = _jwtSecurityTokenHandler.WriteToken(securityToken);
        return token;
    }

    private static string GetLeftMinutes(User user)
    {
        return (user.LockoutEnd!.Value - DateTime.UtcNow).TotalMinutes.ToString("f1", CultureInfo.InvariantCulture);
    }
}
