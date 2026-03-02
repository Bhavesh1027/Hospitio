using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetGuestDefaultCheckinDetails
{
    public class GetGuestDefaultCheckinDetailsOut : BaseResponseOut
    {
        public GetGuestDefaultCheckinDetailsOut(string message, GetGuestDefaultCheckinDetailsResponseOut _getGuestDefaultCheckinDetailsResponseOut) : base(message)
        {
            GetGuestDefaultCheckinDetailsResponseOut = _getGuestDefaultCheckinDetailsResponseOut;
        }
        public GetGuestDefaultCheckinDetailsResponseOut GetGuestDefaultCheckinDetailsResponseOut { get; set; }
    }
    public class GetGuestDefaultCheckinDetailsResponseOut
    {
        public string? CheckInPolicy { get; set; }
        public string? CheckOutPolicy { get; set; }
    }
}
