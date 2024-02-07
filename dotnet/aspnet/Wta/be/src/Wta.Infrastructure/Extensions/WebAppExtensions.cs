using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Extensions;
using Wta.Infrastructure.Web;
using Wta.Shared;

namespace Wta.Infrastructure.Extensions;

public static class WebAppExtensions
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="app"></param>
    public static void Configure(this WebApplication app)
    {
        app.UseRouting();
        app.UseDefaultFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new CompositeFileProvider(app.Services.GetRequiredService<CustomFileProvider>(),
                new ManifestEmbeddedFileProvider(Assembly.GetExecutingAssembly(), "wwwroot"),
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))),
            ContentTypeProvider = app.Services.GetRequiredService<FileExtensionContentTypeProvider>(),
            ServeUnknownFileTypes = true,
        });
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
        app.UseAuthorization();
        app.MapDefaultControllerRoute();
        app.UseCors();
        UseLocalization(app);
        UseDbContext(app);
    }

    private static void UseLocalization(WebApplication app)
    {
        var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
        app.UseRequestLocalization(localizationOptions);
    }

    /// <summary>
    /// 配置数据上下文
    /// </summary>
    /// <param name="application"></param>
    private static void UseDbContext(WebApplication application)
    {
        WebApp.Instance.Assemblies
            .SelectMany(o => o.GetTypes())
            .Where(o => !o.IsAbstract && o.GetBaseClasses().Any(t => t == typeof(DbContext)))
            .OrderBy(o => o.GetCustomAttribute<DisplayAttribute>()?.Order ?? 0)
        .ForEach(dbContextType =>
        {
            using var scope = application.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var contextName = dbContextType.Name;
            if (serviceProvider.GetRequiredService(dbContextType) is DbContext dbContext)
            {
                if (dbContext.Database.EnsureCreated())
                {
                    var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
                    serviceProvider.GetServices(dbSeedType).ForEach(o => dbSeedType.GetMethod("Seed")?.Invoke(o, new object[] { dbContext }));
                }
            }
        });
    }
}
