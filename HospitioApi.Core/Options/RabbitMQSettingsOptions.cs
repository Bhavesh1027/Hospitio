namespace HospitioApi.Core.Options;

public class RabbitMQSettingsOptions
{
    public const string RabbitMQSettings = "RabbitMQSettings";

    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; } 
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Exchange { get; set; } = string.Empty;
    public string CustomerQueue { get; set; } = string.Empty;
    public string GuestMessageQueue { get; set; } = string.Empty;
    public string WPMessageQueue { get; set; } = string.Empty;
   
}
