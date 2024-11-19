using Microsoft.AspNetCore.Builder;
using Wta.Application;
using Wta.Infrastructure.Modules;

public class Program
{
    public static void Main(string[] args)
    {
        //ps>dotnet build ; dotnet ef migrations remove ; dotnet ef migrations add 0 --verbose
        WebApplication.CreateBuilder(args).AddApplication<WtaApplication>().Build();
    }
}
