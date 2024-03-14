using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            foreach (var item in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine(item);
            }
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            builder.Host.UseContentRoot(AppContext.BaseDirectory);
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
            builder.Services.AddSingleton(new CustomFileProvider(Assembly.GetExecutingAssembly(), $"{nameof(WpfApp1)}.wwwroot"));
            var app = builder.Build();
            WebApp = app;
            app.MapGet("/", (CustomFileProvider efp) =>
            {
                return Results.Stream(efp.GetFileInfo("/index.html").CreateReadStream(), "text/html");
            });
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = app.Services.GetRequiredService<CustomFileProvider>()
            }); ;
            app.Services.GetService<IHostApplicationLifetime>()?.ApplicationStarted.Register(() =>
            {
                Url = app.Services.GetService<IServer>()?.Features.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault();
            });
            app.RunAsync();
        }

        public static WebApplication? WebApp { get; private set; }
        public static string? Url { get; private set; }
    }
}