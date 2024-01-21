namespace Wta.Infrastructure.Interfaces;

public interface IImpageCaptchaService
{
    byte[] Create(string code);
}
