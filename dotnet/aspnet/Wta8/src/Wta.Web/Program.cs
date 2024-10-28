using Wta.Application;
using Wta.Infrastructure.Modules;

namespace Wta.Web;

public class Program
{
  public static void Main(string[] args)
  {
    WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build().UseModules().Run();
  }
}
