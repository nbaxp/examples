using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Wta.Application.System.Data;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Modules;

namespace Wta.Application.System;

public class SystemModule : BaseModule
{
  public override void ConfigureServices(WebApplicationBuilder builder)
  {
    builder.AddDbContext<SystemDbContext>();
  }
}
