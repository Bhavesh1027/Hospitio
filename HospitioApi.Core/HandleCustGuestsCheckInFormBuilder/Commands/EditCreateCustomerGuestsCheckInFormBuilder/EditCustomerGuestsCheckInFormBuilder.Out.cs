using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.EditCustomerGuestsCheckInFormBuilder;
public class EditCustomerGuestsCheckInFormBuilderOut : BaseResponseOut
{
    public EditCustomerGuestsCheckInFormBuilderOut(string message, EditedCustomerGuestsCheckInFormBuilderOut editedCustomerGuestsCheckInFormBuilderOut) : base(message)
    {
        EditedCustomerGuestsCheckInFormBuilderOut = editedCustomerGuestsCheckInFormBuilderOut;
    }
    public EditedCustomerGuestsCheckInFormBuilderOut EditedCustomerGuestsCheckInFormBuilderOut { get; set; }

}
public class EditedCustomerGuestsCheckInFormBuilderOut
{
    public int Id { get; set; }
}