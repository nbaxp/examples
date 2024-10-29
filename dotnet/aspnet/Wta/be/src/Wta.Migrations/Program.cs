using Microsoft.AspNetCore.Builder;
using Wta.Application;
using Wta.Infrastructure.Modules;

WebApplication.CreateBuilder(args).AddApplication<ApplicationModule>().Build().Run();
