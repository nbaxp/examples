using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wta.Application.System.Data;
using Wta.Infrastructure.Module;

namespace Wta.Application.System;

public class SystemApplicationModule : BaseApplicationModule
{
    public override void ConfigureModuleServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<SystemDbContext>(
            o => o.UseSqlite(builder.Configuration.GetConnectionString(nameof(SystemDbContext)),
            b =>
            {
                b.UseNetTopologySuite();
                b.MigrationsAssembly("Wta.Migrations");
            }));
    }
}