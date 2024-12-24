namespace Wta.Web.IoTGateway.Infrastructure.Security;

public interface IEncryptionService
{
    string CreateSalt();

    string HashPassword(string password, string salt);

    string EncryptText(string plainText);

    string DecryptText(string cipherText);
}
