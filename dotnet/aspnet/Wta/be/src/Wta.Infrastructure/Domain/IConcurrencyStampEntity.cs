namespace Wta.Infrastructure.Domain;

public interface IConcurrencyStampEntity
{
    string ConcurrencyStamp { get; set; }
}
