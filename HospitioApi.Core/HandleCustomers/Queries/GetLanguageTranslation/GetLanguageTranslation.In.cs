namespace HospitioApi.Core.HandleCustomers.Queries.GetLanguageTranslation;

public class GetLanguageTranslationIn
{
    public int ChannelId { get; set; }
    public string Message { get; set; } = string.Empty;
}
