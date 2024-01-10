namespace Wta.Infrastructure.Web;

public class CustomApiResponse<T>
{
    public CustomApiResponse()
    {
    }

    public CustomApiResponse(T? data, int code = 0, string? message = null)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public int Code { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}

public class CustomApiResponse : CustomApiResponse<object>
{
    public static CustomApiResponse<T> Create<T>(T? data, int code = 0, string? message = null)
    {
        return new CustomApiResponse<T>(data, code, message);
    }
}
