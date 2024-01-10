namespace Wta.Infrastructure.Models;

public class ExportModel<T> : QueryModel<T>
{
    public bool IncludeAll { get; set; }
}
