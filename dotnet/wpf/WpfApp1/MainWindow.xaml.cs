using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool Rendered { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            await this.EnsureCoreWebView2Async();
            if (!App.WebApp!.Services.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                webView.CoreWebView2.Settings.IsPinchZoomEnabled = false;
                webView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
                webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
            }
            webView.CoreWebView2.Navigate(App.Url);
        }

        private async Task EnsureCoreWebView2Async()
        {
            if (this.Rendered)
            {
                return;
            }
            this.Rendered = true;
            await webView.EnsureCoreWebView2Async(await CoreWebView2Environment.CreateAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebView2")));
        }
    }
}