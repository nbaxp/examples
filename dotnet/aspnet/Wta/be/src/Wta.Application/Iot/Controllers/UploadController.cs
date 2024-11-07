using System.Text.Json;

namespace Wta.Application.Iot.Controllers;

[AllowAnonymous]
public class UploadController : Controller
{
    public async Task<IActionResult> Mqtt()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync().ConfigureAwait(false);
        var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
        Console.WriteLine(body.ToJson());
        return Problem();
        //return Ok();
    }
}
