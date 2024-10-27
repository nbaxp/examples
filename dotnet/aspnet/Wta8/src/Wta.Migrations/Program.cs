using Wta.Application;
using Wta.Infrastructure;

WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build().UseModules().Run();
