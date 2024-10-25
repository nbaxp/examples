using Wta.Application.System;
using Wta.Infrastructure.Module;

WebApplication.CreateBuilder(args).AddModule<SystemApplicationModule>().Build().UseModules().Run();