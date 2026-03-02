using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerLatitudeLongitude;

public class GetCustomerLatitudeLongitudeOut : BaseResponseOut
{
    public GetCustomerLatitudeLongitudeOut(string message, CustomerLatitudeLongitude response) : base(message)
    {
        CustomerLatitudeLongitude = response;
    }
    public CustomerLatitudeLongitude CustomerLatitudeLongitude { get; set; }
}
public class CustomerLatitudeLongitude
{
    public string? BusinessName { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set;}
}