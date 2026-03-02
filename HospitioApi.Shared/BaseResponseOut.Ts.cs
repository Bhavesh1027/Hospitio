namespace HospitioApi.Shared;
public class BaseResponseOut
{
    public BaseResponseOut(string message, List<string>? errors = null)
    {
        Message = message;
        Errors = errors ?? new();
    }

    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}