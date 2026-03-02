using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequest;

public class UpdateGuestRequestIn
{
    public int Id { get; set; }
    public GuestRequestStatusEnum Status { get; set; }
    public bool IsActive { get; set; }
}
