using HospitioApi.Shared;

namespace HospitioApi.Core.HandleHospitioOnBoarding.Queries.GetHospitioOnBoarding;

public class GetHospitioOnBoardingOut : BaseResponseOut
{
    public GetHospitioOnBoardingOut(string message, HospitioOnBoardingOut hospitioOnBoardingOut) : base(message)
    {
        HospitioOnBoardingOut = hospitioOnBoardingOut;
    }
    public HospitioOnBoardingOut HospitioOnBoardingOut { get; set; }
}
public class HospitioOnBoardingOut
{
    public int Id { get; set; }
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
