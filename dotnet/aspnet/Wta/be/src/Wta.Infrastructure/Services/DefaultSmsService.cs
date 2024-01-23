using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Configuration;
using Wta.Infrastructure.Interfaces;

namespace Wta.Infrastructure.Services;

[Service<ISmsService>]
public class DefaultSmsService(IHttpClientFactory httpClientFactory, IOptions<SmsOptions> options) : ISmsService
{
    public void Send(string phoneNumber, out string code)
    {
        using var sha1 = SHA1.Create();
        var url = options.Value.Url;
        var key = options.Value.Key;
        var secret = options.Value.Secret;
        var client = httpClientFactory.CreateClient();
        var now = $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        var nonce = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);
        var checkSum = BitConverter.ToString(sha1.ComputeHash(Encoding.ASCII.GetBytes($"{secret}{nonce}{now}"))).Replace("-", "").ToLowerInvariant();
        client.DefaultRequestHeaders.Add("AppKey", key);
        client.DefaultRequestHeaders.Add("CurTime", now);
        client.DefaultRequestHeaders.Add("Nonce", nonce);
        client.DefaultRequestHeaders.Add("CheckSum", checkSum);
        client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded;charset=utf-8");
        using var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("mobile", phoneNumber) });
        var result = client.PostAsync(url, content).Result;
        var responseContent = result.Content.ReadAsStringAsync().Result;
        var returnModel = JsonSerializer.Deserialize<ReturnModel>(responseContent);
        code = returnModel?.Obj!;
    }

    private static char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

    public class ReturnModel
    {
        public string? Code { get; set; }
        public string? Msg { get; set; }
        public string? Obj { get; set; }
    }
}
