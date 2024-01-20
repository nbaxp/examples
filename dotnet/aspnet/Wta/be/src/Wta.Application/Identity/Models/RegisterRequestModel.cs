using Microsoft.AspNetCore.Mvc;
using Wta.Infrastructure.Interfaces;
using Wta.Shared;

namespace Wta.Application.Identity.Models;

public class RegisterRequestModel : IValidatableObject
{
    [Required]
    [RegularExpression("[a-zA-Z0-9_-]")]
    [Remote("ValidUserName", "User")]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Code { get; set; }

    [Required]
    [HiddenInput]
    public string? CodeHash { get; set; }

    [Required]
    public string? Password { get; set; }

    [Compare("Password")]
    public string? ConfirmPassword { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        using var scope = WebApp.Instance.WebApplication.Services.CreateScope();
        var encryptionService = scope.ServiceProvider.GetRequiredService<IEncryptionService>();
        if (encryptionService.DecryptText(this.CodeHash!) != this.Code)
        {
            yield return new ValidationResult("Error Code", ["Code"]);
        }
        try
        {
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
