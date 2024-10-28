using Microsoft.AspNetCore.Builder;

namespace Wta.Infrastructure;

public static class Global
{
  public static WebApplication Application { get; set; } = default!;
}
