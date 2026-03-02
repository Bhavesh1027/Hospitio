
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;


public class GetCustomerGuestsCheckInFormBuilderOut : BaseResponseOut
{
    public GetCustomerGuestsCheckInFormBuilderOut(string message, GetCustomerGuestsCheckInFormBuilderResponseOut getCustomerGuestsCheckInFormBuilderResponseOut) : base(message)
    {
        GetCustomerGuestsCheckInFormBuilderResponseOut = getCustomerGuestsCheckInFormBuilderResponseOut;
    }
    public GetCustomerGuestsCheckInFormBuilderResponseOut GetCustomerGuestsCheckInFormBuilderResponseOut { get; set; }

}
public class GetCustomerGuestsCheckInFormBuilderResponseOut
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? Color { get; set; }
    public string? Name { get; set; }
    public byte? Stars { get; set; }
    public string? Logo { get; set; }
    public string? AppImage { get; set; }
    public string? SplashScreen { get; set; }
    public bool IsOnlineCheckInFormEnable { get; set; } = true;
    public bool IsRedirectToGuestAppEnable { get; set; } = true;
    public string? SubmissionMail { get; set; }
    public string? TermsLink { get; set; }
    public bool IsActive { get; set; } = true;
    public string? GuestWelcomeMessage { get; set; }
    public List<GetCustomerGuestsCheckInFormFieldsOut> GetCustomerGuestsCheckInFormFieldsOut { get; set; } = new();
}


public class GetCustomerGuestsCheckInFormFieldsOut
{
    public string Name { get; set; } = String.Empty;
    public string FieldType { get; set; } = String.Empty;
    public bool RequiredFields { get; set; }
    public bool IsActive { get; set; } = true;
    public int? DisplayOrder { get; set; }
}