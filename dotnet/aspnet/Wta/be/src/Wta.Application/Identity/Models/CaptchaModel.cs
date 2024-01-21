using System.Globalization;
using Wta.Infrastructure.Attributes;
using Wta.Infrastructure.Interfaces;
using Wta.Shared;

namespace Wta.Application.Identity.Models;

public class CaptchaModel : IValidatableObject
{
    [Required]
    public string? Code { get; set; }

    [Required, Hidden]
    public string? CodeHash { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        using var scope = WebApp.Instance.WebApplication.Services.CreateScope();
        var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();
        var values = encryptionService.DecryptText(CodeHash!).Split(',');
        var timeout = DateTime.Parse(values[0], CultureInfo.InvariantCulture);
        var code = values[1];
        if (DateTime.UtcNow > timeout)
        {
            yield return new ValidationResult("CaptchaErrorTimeout", [nameof(Code)]);
        }
        if (code != Code)
        {
            yield return new ValidationResult("CaptchaError", [nameof(Code)]);
        }
    }
}
