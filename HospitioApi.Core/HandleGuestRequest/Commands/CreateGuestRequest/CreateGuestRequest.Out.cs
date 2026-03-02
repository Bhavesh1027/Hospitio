using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;

public class CreateGuestRequestOut : BaseResponseOut
{
    public CreateGuestRequestOut(string message) : base(message)
    {
    }
}
