using Microsoft.AspNetCore.Builder;
using Wta.Application.System;
using Wta.Infrastructure;

namespace Wta.Application;

public class WtaApplication : BaseApplication
{
  public override void ConfigureServices(WebApplicationBuilder builder)
  {
    base.ConfigureServices(builder);
    builder.AddModule<SystemModule>();
  }
}
