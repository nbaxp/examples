namespace Wta.Infrastructure.Interfaces;

public interface ISmsService
{
    void Send(string phoneNumber, out string code);
}
