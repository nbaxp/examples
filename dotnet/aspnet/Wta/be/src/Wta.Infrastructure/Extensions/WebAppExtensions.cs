using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Wta.Infrastructure.Data;
using Wta.Infrastructure.Exceptions;
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
        //app.UseExceptionHandler(c => c.Run(async context =>
        //{
        //    var exception = context.Features
        //        .Get<IExceptionHandlerPathFeature>()
        //        .Error;
        //    var response = new { error = exception.Message };
        //    await context.Response.WriteAsJsonAsync(response);
        //}));
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
            if (serviceProvider.GetRequiredService(dbContextType) is DbContext initDbContext)
            {
                var dbCreator = (initDbContext.GetService<IRelationalDatabaseCreator>() as RelationalDatabaseCreator)!;
                if (!dbCreator.Exists())
                {
                    dbCreator.Create();
                    var createSql = "CREATE TABLE EFDbContext(Id varchar(255) NOT NULL,Hash varchar(255),Date datetime  NOT NULL,PRIMARY KEY (Id));";
                    initDbContext.Database.ExecuteSqlRaw(createSql);
                }
            }
            if (serviceProvider.GetRequiredService(dbContextType) is DbContext context)
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
                    var dbCreator = (context.GetService<IRelationalDatabaseCreator>() as RelationalDatabaseCreator)!;
                    var sql = dbCreator.GenerateCreateScript();
                    var md5 = sql.ToMd5();
                    var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location!)!, "scripts");
                    Directory.CreateDirectory(path);
                    using var sw = File.CreateText(Path.Combine(path, $"db.{context.Database.ProviderName}.{contextName}.sql"));
                    sw.Write(sql);
                    Console.WriteLine($"{contextName} 初始化开始");
                    Console.WriteLine($"ConnectionString:{context.Database.GetConnectionString()}");
                    // 查询当前DbContext是否已经初始化
                    var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    var connection = context.Database.GetDbConnection();
                    var command = connection.CreateCommand();
                    command.Transaction = transaction.GetDbTransaction();
                    command.CommandText = $"SELECT Hash FROM EFDbContext where Id='{contextName}'";
                    var hash = command.ExecuteScalar();
                    if (hash == null)
                    {
                        if (context.Database.ProviderName!.Contains("SqlServer"))
                        {
                            var pattern = @"(?<=;\s+)GO(?=\s\s+)";
                            var sqls = Regex.Split(sql, pattern).Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
                            foreach (var item in sqls)
                            {
                                command.CommandText = Regex.Replace(sql, pattern, string.Empty);
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            command.CommandText = sql;
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = $"INSERT INTO EFDbContext VALUES ('{contextName}', '{md5}','{now}');";
                        command.ExecuteNonQuery();
                        var dbSeedType = typeof(IDbSeeder<>).MakeGenericType(dbContextType);
                        serviceProvider.GetServices(dbSeedType).ForEach(o => dbSeedType.GetMethod("Seed")?.Invoke(o, new object[] { context }));
                        Console.WriteLine($"{contextName} 初始化成功");
                    }
                    else
                    {
                        Console.WriteLine($"{contextName} 数据库结构{(hash.ToString() == md5 ? "正常" : "已过时")}");
                    }
                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var message = $"{contextName} 初始化失败：{ex.Message}";
                    Console.WriteLine(message);
                    Console.WriteLine(ex.ToString());
                    throw new ProblemException(message, ex);
                }
                finally
                {
                    Console.WriteLine($"{contextName} 初始化结束");
                }
            }
        });
    }
}
