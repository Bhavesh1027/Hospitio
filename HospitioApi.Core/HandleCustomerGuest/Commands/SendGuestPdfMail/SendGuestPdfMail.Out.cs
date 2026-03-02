using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.SendGuestPdfMail;

public class SendGuestPdfMailOut : BaseResponseOut
{
    public SendGuestPdfMailOut(string message) : base(message)
    {

    }
}

