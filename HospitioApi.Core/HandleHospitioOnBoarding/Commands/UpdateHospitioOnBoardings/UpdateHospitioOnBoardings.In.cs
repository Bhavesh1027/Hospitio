namespace HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;

public class UpdateHospitioOnBoardingsIn
{
    public int Id { get; set; } = 1;
    public string? WhatsappCountry { get; set; }
    public string? WhatsappNumber { get; set; }
    public string? ViberCountry { get; set; }
    public string? ViberNumber { get; set; }
    public string? TelegramCountry { get; set; }
    public string? TelegramNumber { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public string? SmsTitle { get; set; }
    public string? Messenger { get; set; }
    public string? Email { get; set; }
    public string? Cname { get; set; }
    public string? IncomingTranslationLanguage { get; set; }
    public string? NoTranslateWords { get; set; }
}
