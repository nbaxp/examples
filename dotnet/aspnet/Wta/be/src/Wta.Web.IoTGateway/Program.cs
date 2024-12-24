using Microsoft.EntityFrameworkCore;
using Wta.Web.IoTGateway.Data;
using Wta.Web.IoTGateway.Domain;
using Wta.Web.IoTGateway.Infrastructure.Drivers;
using Wta.Web.IoTGateway.Infrastructure.Drivers.Plc.Melsec;
using Wta.Web.IoTGateway.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString(nameof(ApplicationDbContext));

builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(connectionString).UseSeeding((context, _) =>
{
    var user = new User { UserName = "admin", Salt = "admin" };
    user.PasswordHash = new EncryptionService(builder.Configuration).HashPassword("123456", user.Salt);
    context.Set<User>().Add(user);
    //
    var driverTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(o => o.GetTypes())
    .Where(o => o.IsAssignableTo(typeof(IDriver)) && !o.IsAbstract)
    .ToList();
    foreach (var type in driverTypes)
    {
        context.Set<Driver>().Add(new Driver { Name = type.Name, Value = type.FullName! });
    }
    //
    context.Set<Device>().Add(new Device
    {
        Name = "三菱Fx",
        Number = "001",
        Driver = typeof(MelsecFxSerialOverTcp).FullName!,
        Datas = new List<Data>
        {
            new() { Key="test", Address="D001", ByteLength = 2 }
        }
    });
    context.SaveChanges();
}));
builder.Services.AddScoped<DbContext, ApplicationDbContext>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();

var app = builder.Build();

app.MapGet("/", () => Results.File("~/index.html", "text/html"));

app.MapStaticAssets();

using var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

app.Run();
