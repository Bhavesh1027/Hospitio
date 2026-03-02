using Vonage.Numbers;

namespace HospitioApi.Core.Services.Vonage;

public interface IVonageService
{
    Task<dynamic> CreateVonageSubAccount(string subAccountName, int customerId);
    Task<dynamic> GetVonageSubAccount();
    Task<dynamic> CreateApplication(string applicationName, int customerId);
    Task<dynamic> GetApplications(int pageNo, int pageSize);
    Task<dynamic> SendWhatsappTextMessage(string appId, string privatKey, string senderNumber, string ReceiverNumber, string message);
    Task<dynamic> SendWhatsappAudioMessage(string appId, string privatKey, string message);
    Task<dynamic> SendWhatsappFileMessage(string appId, string privatKey, string message);
    Task<dynamic> SendWhatsappImageMessage(string appId, string privatKey, string message);
    Task<dynamic> SendWhatsappVideoMessage(string appId, string privatKey, string message);
    Task<dynamic> SendWhatsappTemplateMessage(string appId, string privatKey, string message,string receiver,string sender,string templateName,List<string> BodyParameters, bool hasButton, Dictionary<int,string> ButtonParameters);

    Task<dynamic> SendSMS(string appId, string privatKey, string SenderNumber, string ReceiverNumber, string message);
    Task<string> GenerateJWT(string appId, string privatKey);
    Task<VonageTemplateReponseDto> CreateTemplate(string appId, int Id ,string privatKey,string WABAId ,dynamic templateData, CancellationToken cancellationToken,string UserType, string stepName, int CustomerId);
    Task<VonageTemplateReponseDto> UpdateTemplate(string appId, string privatKey, string WABAId, string templateId, dynamic templateData, CancellationToken cancellationToken,string UserType, string TemplateName , int CustomerId);
    Task<dynamic> RemoveWhatsappTemplate(string appId, string privatKey, string WABAId, string TemplateName, string VonageTemplateId);
    Task<dynamic> LinkApplicationToAccount(string appId, string privatKey, string Provider, string ExternalId);
    Task<dynamic> UnlinkApplicationFromAccount(string appId, string privatKey, string Provider, string ExternalId);
    Task<ResultForRetriveWhatsappAccount> RetriveWhatsappAccount(string appId, string privatKey, string WhatsappNumber);
    Task<NumberTransactionResponse> BuyNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber);
    Task<NumberTransactionResponse> CancelNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber);
    Task<NumbersSearchResponse> ListOwnedNumbers(string vonageApiKey, string vonageApiSecret, string? numberSearchCriteria, SearchPattern? numberSearchPattern);
    Task<NumbersSearchResponse> SearchNumbers(string vonageApiKey, string vonageApiSecret, string? countryCode, string? vonageNumberType, string? vonageNumberFeatures, string? numberSearchCriteria, SearchPattern? numberSearchPattern, int? size, int? Index);
    Task<NumberTransactionResponse> UpdateNumber(string vonageApiKey, string vonageApiSecret, string countryCode, string vonageNumber, string vonageApplicationId);
}
public class VonageTemplateReponseDto
{
    public string Status { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public dynamic Buttons { get; set; } = string.Empty;
    public string TemplateName { get; set; } = string.Empty;
    public string TemplateStatus { get; set; } = string.Empty;
}
