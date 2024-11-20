using System.ComponentModel;
using System.Diagnostics;
using Serilog;

namespace Wta.Desktop;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private bool fullScreen;

    [DefaultValue(false)]
    public bool FullScreen
    {
        get { return fullScreen; }
        set
        {
            fullScreen = value;
            if (value)
            {
                this.WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.Activate();
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Normal;
            }
        }
    }

    private Process? web;

    private void Form1_LoadAsync(object sender, EventArgs e)
    {
        webView21.Source = new Uri(@$"file:///{Path.Combine(Application.StartupPath, "wwwroot", "index.html")}");
        Process.GetProcessesByName("Wta.Web").FirstOrDefault()?.Kill(true);
        var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publish", "Wta.Web.exe");
        web = new Process
        {
            StartInfo = new ProcessStartInfo(file)
            {
                WorkingDirectory = Path.GetDirectoryName(file),
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
            }
        };
        web.OutputDataReceived += OutputDataReceived;
        web.Start();
        web.BeginOutputReadLine();
        web.WaitForExitAsync().ConfigureAwait(false);
        using var hc = new HttpClient();
        while (true)
        {
            try
            {
                var result = hc.GetAsync("http://localhost:5000/api/metrics").Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    webView21.EnsureCoreWebView2Async().ContinueWith(o =>
                    {
                        webView21.Invoke(() =>
                        {
                            webView21.CoreWebView2.ContainsFullScreenElementChanged += (obj, args) =>
                            {
                                this.FullScreen = webView21.CoreWebView2.ContainsFullScreenElement;
                            };
                            webView21.Source = new Uri("http://localhost:5000");
                        });
                    }, TaskScheduler.Default);
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }

    private void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Log.Information(e.Data);
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        web?.Kill(true);
    }
}
