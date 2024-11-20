using System.Diagnostics;
using System.Globalization;
using Serilog;

namespace Wta.Desktop;

internal static class Program
{
    private static Mutex? mutex;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        var name = Process.GetCurrentProcess().ProcessName;
        mutex = new Mutex(true, name, out bool isFirstInstance);
        if (isFirstInstance)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day, formatProvider: CultureInfo.InvariantCulture)
                .CreateLogger();
            try
            {
                Log.Information("application start:");
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
                Log.Information("application ended!");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        else
        {
            MessageBox.Show($"{name} 已经在运行");
        }
        mutex?.ReleaseMutex();
    }
}
