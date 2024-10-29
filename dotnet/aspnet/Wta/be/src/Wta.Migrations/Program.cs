using Wta.Application;
using Wta.Infrastructure.Modules;

//ps>dotnet ef migrations remove ; dotnet ef migrations add 0 --verbose
WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build().UseModules().Run();
