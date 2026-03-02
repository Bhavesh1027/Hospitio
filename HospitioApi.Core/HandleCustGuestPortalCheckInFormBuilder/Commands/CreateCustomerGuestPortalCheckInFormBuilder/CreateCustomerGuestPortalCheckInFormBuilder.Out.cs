using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.CreateCustomerGuestPortalCheckInFormBuilder;

public class CreateCustomerGuestPortalCheckInFormBuilderOut : BaseResponseOut
{
    public CreateCustomerGuestPortalCheckInFormBuilderOut(string message, CreatedCustomerGuestsCheckInFormBuilderOut createdCustomerGuestsCheckInFormBuilderOut) : base(message)
    {
        CreatedCustomerGuestsCheckInFormBuilderOut = createdCustomerGuestsCheckInFormBuilderOut;
    }
    public CreatedCustomerGuestsCheckInFormBuilderOut CreatedCustomerGuestsCheckInFormBuilderOut { get; set; }
}
public class CreatedCustomerGuestsCheckInFormBuilderOut
{
    public int GuestId { get; set; }
    public string Link { get; set; } = string.Empty;
}