using Hangfire;
using MQTTnet.AspNetCore;
using Prometheus;
using Serilog;
using Wta.Infrastructure.Mqtt;
using Wta.Infrastructure.Tenant;

namespace Wta.Infrastructure.Modules;

public abstract partial class BaseApplication
{
    public virtual void UseMqttServer(WebApplication app)
    {
        var handler = app.Services.GetRequiredService<MqttServerHandler>();
        app.UseMqttServer(server =>
        {
            server.ValidatingConnectionAsync += handler.ValidateConnectionAsync;
            server.ClientConnectedAsync += handler.OnClientConnectedAsync;
            server.InterceptingPublishAsync += handler.InterceptingPublishAsync;
        });
        app.MapConnectionHandler<MqttConnectionHandler>("/mqtt", o =>
        {
            o.WebSockets.SubProtocolSelector = protocolList => protocolList.FirstOrDefault() ?? string.Empty;
        });
    }

    public virtual void UseMonitoring(WebApplication app)
    {
        app.MapHealthChecks("/hc");
        app.UseMetricServer("/api/metrics").UseHttpMetrics();
    }

    public virtual void UseLogging(WebApplication app)
    {
        app.UseSerilogRequestLogging(o =>
        {
            o.MessageTemplate = "[{TenantNumber}:{UserName}] HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            o.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                var tenantNumber = httpContext.User.Claims.FirstOrDefault(o => o.Type == "TenantNumber")?.Value ?? TenantConstants.ROOT;
                var userName = httpContext.User.Identity?.Name;
                diagnosticContext.Set("TenantNumber", tenantNumber);
                diagnosticContext.Set("UserName", userName);
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("ContentType", httpContext.Response.ContentType);
            };
        });
    }

    public virtual void UseStaticFiles(WebApplication app)
    {
        //app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = app.Services.GetRequiredService<IFileProvider>(),
            //FileProvider = new CompositeFileProvider(app.Services.GetRequiredService<CustomFileProvider>(),
            //    new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot"),
            //    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))),
            ContentTypeProvider = app.Services.GetRequiredService<FileExtensionContentTypeProvider>(),
            ServeUnknownFileTypes = true,
        });
    }

    public virtual void UseRouting(WebApplication app)
    {
        app.UseRouting();
    }

    public virtual void UseOpenApi(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var apiDescriptionGroups = app.Services.GetRequiredService<IApiDescriptionGroupCollectionProvider>().ApiDescriptionGroups.Items;
                foreach (var description in apiDescriptionGroups)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                }
            });
        }
    }

    public virtual void UseAuth(WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    public virtual void UseEndpoints(WebApplication app)
    {
        app.MapDefaultControllerRoute();
    }

    public virtual void UseSignalR(WebApplication app)
    {
        app.MapHub<DefaultHub>("/api/hub");
    }

    [Description("UseCors 必须在 MapHub 之后")]
    public virtual void UseCORS(WebApplication app)
    {
        app.UseCors(Constants.CORS_POLICY);
    }

    public virtual void UseLocalization(WebApplication app)
    {
        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
        app.UseRequestLocalization(localizationOptions);
    }

    public virtual void UseDbContext(WebApplication app)
    {
        return;
        //AppDomain.CurrentDomain.GetCustomerAssemblies()
        //    .SelectMany(o => o.GetTypes())
        //    .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
        //    .OrderBy(o => o.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0)
        //    .ForEach(dbContextType =>
        //    {
        //        using var scope = app.Services.CreateScope();
        //        var serviceProvider = scope.ServiceProvider;
        //        var contextName = dbContextType.Name;
        //        if (serviceProvider.GetRequiredService(dbContextType) is DbContext dbContext)
        //        {
        //            if (dbContext.Database.EnsureCreated())
        //            {
        //                var @lock = app.Services.GetRequiredService<ILock>();
        //                using var handle = @lock.Acquire("seed");
        //                if (handle != null)
        //                {
        //                    var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
        //                    serviceProvider.GetServices(dbSeedType)
        //                    .OrderBy(o => o!.GetType().GetAttribute<DisplayAttribute>()?.GetOrder() ?? 0)
        //                    .ForEach(o =>
        //                    {
        //                        dbSeedType.GetMethod(nameof(IDbSeeder<DbContext>.Seed))?.Invoke(o, [dbContext]);
        //                    });
        //                }
        //            }
        //        }
        //    });
    }

    public virtual void UseScheduler(WebApplication webApplication)
    {
        webApplication.UseHangfireDashboard();
    }
}
