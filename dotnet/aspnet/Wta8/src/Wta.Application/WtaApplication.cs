using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wta.Application.System;
using Wta.Infrastructure.Modules;

namespace Wta.Application;

public class WtaApplication : BaseApplication
{
  public override void ConfigureServices(WebApplicationBuilder builder)
  {
    base.ConfigureServices(builder);
    builder.AddModule<SystemModule>();
  }

  //public override void Configure(WebApplication app)
  //{
  //  base.Configure(app);
  //  if (app.Environment.IsDevelopment())
  //  {
  //    using var scope = app.Services.CreateScope();
  //    var list = scope.ServiceProvider.GetServices<DbContext>();
  //    foreach (var db in list)
  //    {
  //      if (db.Database.EnsureCreated())
  //      {
  //        db.Database.Migrate();
  //      }
  //    }
  //  }
  //}
}
