using Microsoft.AspNetCore.Http;

namespace Wta.Infrastructure.Application.Models;

public class ImportModel<T> : QueryModel<T>
{
    public string Update { get; set; } = null!;
    public List<IFormFile> Files { get; set; } = new List<IFormFile>();
}
