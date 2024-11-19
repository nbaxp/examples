using System.Diagnostics;

namespace Wta.Desktop;

internal static class Program
{
    private static Mutex? mutex;
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        var name = Process.GetCurrentProcess().ProcessName;
        mutex = new Mutex(true, name, out bool isFirstInstance);
        if (isFirstInstance)
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        else {
            MessageBox.Show($"{name} 已经在运行");
        }
        mutex?.ReleaseMutex();
    }
}
