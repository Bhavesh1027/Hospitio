using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;

public class UpdateIsActiveCustomersDigitalAssistantsOut : BaseResponseOut
{
    public UpdateIsActiveCustomersDigitalAssistantsOut(string message, UpdatedIsActiveCustomersDigitalAssistantsOut updatedIsActiveCustomersDigitalAssistantsOut) : base(message)
    {
        UpdatedIsActiveCustomersDigitalAssistantsOut = updatedIsActiveCustomersDigitalAssistantsOut;
    }
    public UpdatedIsActiveCustomersDigitalAssistantsOut UpdatedIsActiveCustomersDigitalAssistantsOut { get; set; }
}
public class UpdatedIsActiveCustomersDigitalAssistantsOut
{
    public int Id { get; set; }
    public bool? IsActive { get; set; }
}