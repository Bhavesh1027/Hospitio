using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonage.Commands.Vonage;

public class VonageOut : BaseResponseOut
{
    public VonageOut(string message) : base(message) { }
}
