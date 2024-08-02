namespace Wta.Infrastructure.Application.Models;

public class ExportModel<T> : QueryModel<T>
{
    public string Format { get; set; } = null!;
}
