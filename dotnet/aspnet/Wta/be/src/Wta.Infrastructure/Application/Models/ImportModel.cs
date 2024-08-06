using Microsoft.AspNetCore.Http;

namespace Wta.Infrastructure.Application.Models;

public class ImportModel<T>
{
    public string Update { get; set; } = null!;
    public IFormFile File { get; set; } = null!;
}
