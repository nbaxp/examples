using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Wta.Application.Oee.Resources;
using Wta.Infrastructure.Monitoring;

namespace Wta.Application.Platform.Controllers;

public class OeeAnalysisController() : BaseController, IResourceService<OeeAnalysis>
{
    [Display(Name = "OEE分析")]
    [Authorize]
    [HttpGet]
    public async Task Index()
    {
        Response.ContentType = "text/event-stream";
        var process = Process.GetCurrentProcess();
        while (!this.HttpContext.RequestAborted.IsCancellationRequested)
        {
            var addresses = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(o => o.AddressFamily == AddressFamily.InterNetwork)
            .Select(o => o.ToString())
            .ToArray();
            var memoryTotal = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
            var drive = DriveInfo.GetDrives().FirstOrDefault(o => o.RootDirectory.FullName == Directory.GetDirectoryRoot(Path.GetPathRoot(Environment.ProcessPath!)!))!;
            var line = new MonitorModel
            {
                //os
                OSArchitecture = RuntimeInformation.OSArchitecture.ToString(),
                OSDescription = RuntimeInformation.OSDescription,
                UserName = Environment.UserName,
                ServerTime = DateTime.UtcNow,
                HostName = Dns.GetHostName(),
                HostAddresses = string.Join(',', addresses),
                RuntimeIdentifier = RuntimeInformation.RuntimeIdentifier,
                //cpu
                CpuCount = Environment.ProcessorCount,
                CpuUsage = EventListenerHostedService.Data.GetValueOrDefault("cpu-usage"),
                //memory
                MemoryTotal = memoryTotal,
                MemoryUsage = EventListenerHostedService.Data.GetValueOrDefault("working-set") * 1024 * 1024 / memoryTotal,
                //network
                BytesReceived = (int)EventListenerHostedService.Data.GetValueOrDefault("bytes-received"),
                BytesSent = (int)EventListenerHostedService.Data.GetValueOrDefault("bytes-sent"),
                //disk
                //framework
                //app

                ProcessCount = Process.GetProcesses().Length,
                ProcessArchitecture = RuntimeInformation.ProcessArchitecture.ToString(),
                ProcessName = process.ProcessName,
                ProcessArguments = string.Join(' ', Environment.GetCommandLineArgs()),
                ProcessId = process.Id,
                ProcessHandleCount = process.HandleCount,
                ProcessFileName = Environment.ProcessPath,
                DriveName = drive.Name,
                DrivieTotalSize = drive.TotalSize,
                DriveAvailableFreeSpace = drive.AvailableFreeSpace,
                Framework = RuntimeInformation.FrameworkDescription,
                ExceptionCount = (int)EventListenerHostedService.Data.GetValueOrDefault("exception-count"),
                TotalRequests = (int)EventListenerHostedService.Data.GetValueOrDefault("total-requests"),
                CurrentRequests = (int)EventListenerHostedService.Data.GetValueOrDefault("current-requests"),
            };
            var message = "data:" + JsonSerializer.Serialize(line) + "\n\n";
            await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(message)).ConfigureAwait(false);
            await Response.Body.FlushAsync().ConfigureAwait(false);
            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}
