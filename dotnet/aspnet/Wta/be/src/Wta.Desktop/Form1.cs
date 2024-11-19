using System.ComponentModel;
using System.Diagnostics;

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
        var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "publish", "Wta.Web.exe");
        web = new Process();
        web.StartInfo = new ProcessStartInfo(file)
        {
            //UseShellExecute = false,
            //CreateNoWindow = true,
            //WindowStyle = ProcessWindowStyle.Hidden,
            RedirectStandardOutput = true,
        };
        web.OutputDataReceived += OutputDataReceived;
        web.Start();
        web.BeginOutputReadLine();
        web.WaitForExitAsync();
    }

    private async void OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        Debug.WriteLine(e.Data);
        //webView21.Source = new Uri("http://localhost:5000");
        //await webView21.EnsureCoreWebView2Async();
        //webView21.CoreWebView2.ContainsFullScreenElementChanged += (obj, args) =>
        //{
        //    this.FullScreen = webView21.CoreWebView2.ContainsFullScreenElement;
        //};
    }

    private void Form1_FormClosed(object sender, FormClosedEventArgs e)
    {
        web?.Close();
    }
}
