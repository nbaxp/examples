using Wta.Application;
using Wta.Infrastructure.Modules;

WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build().UseModules().Run();
