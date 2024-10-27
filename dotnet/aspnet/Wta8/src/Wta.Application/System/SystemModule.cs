using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wta.Application.System.Data;
using Wta.Infrastructure;
using Wta.Infrastructure.Data;

namespace Wta.Application.System;

public class SystemModule : BaseModule
{
  public override void ConfigureServices(WebApplicationBuilder builder)
  {
    builder.AddDbContext<SystemDbContext>(nameof(SystemDbContext), "Wta.Migrations");
  }
}
