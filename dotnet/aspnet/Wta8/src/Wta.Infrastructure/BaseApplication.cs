using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Wta.Infrastructure;

public abstract class BaseApplication : IApplication
{
  public virtual void ConfigureServices(WebApplicationBuilder builder)
  {
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
  }

  public virtual void Configure(WebApplication app)
  {
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }
    app.UseAuthorization();
    app.MapControllers();
  }
}
