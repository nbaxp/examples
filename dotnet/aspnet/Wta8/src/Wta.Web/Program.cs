using Wta.Application;
using Wta.Infrastructure;

namespace Wta.Web;

public class Program
{
  public static void Main(string[] args)
  {
    WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build().UseModules().Run();
  }
}
