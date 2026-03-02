using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneys;

public class UpdateIsActiveCustomersGuestJourneysOut : BaseResponseOut
{
    public UpdateIsActiveCustomersGuestJourneysOut(string message, UpdatedIsActiveCustomersGuestJourneysOut updatedIsActiveCustomersGuestJourneysOut) : base(message)
    {
        UpdatedIsActiveCustomersGuestJourneysOut = updatedIsActiveCustomersGuestJourneysOut;
    }
    public UpdatedIsActiveCustomersGuestJourneysOut UpdatedIsActiveCustomersGuestJourneysOut { get; set; }
}
public class UpdatedIsActiveCustomersGuestJourneysOut
{
    public int Id { get; set; }
    public bool? IsActive { get; set; }
}
