using Microsoft.AspNetCore.SignalR;

var CorsPolicy = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, builder =>
    {
        builder.SetIsOriginAllowed(isOriginAllowed => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapGet("/", (HttpContext context) => context.Request.Headers);
app.MapHub<ChatHub>("/chatHub");
app.UseCors(CorsPolicy);
app.Run();

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}