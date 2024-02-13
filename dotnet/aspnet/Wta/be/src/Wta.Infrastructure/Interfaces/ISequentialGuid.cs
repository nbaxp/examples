namespace Wta.Infrastructure.Interfaces;

public interface ISequentialGuid
{
    Guid Create(string type);
}
