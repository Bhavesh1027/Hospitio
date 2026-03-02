using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoById;

public class GetCustomersPropertiesInfoByIdOut : BaseResponseOut
{
    public GetCustomersPropertiesInfoByIdOut(string message, CustomersPropertiesInfoByIdOut customersPropertiesInfoByIdOut) : base(message)
    {
        CustomersPropertiesInfoByIdOut = customersPropertiesInfoByIdOut;
    }
    public CustomersPropertiesInfoByIdOut CustomersPropertiesInfoByIdOut { get; set; }
}
public class CustomersPropertiesInfoByIdOut
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    public string? WifiUsername { get; set; }
    public string? WifiPassword { get; set; }
    public string? Overview { get; set; }
    public string? CheckInPolicy { get; set; }
    public string? TermsAndConditions { get; set; }
    public string? Street { get; set; }
    public string? StreetNumber { get; set; }
    public string? City { get; set; }
    public string? Postalcode { get; set; }
    public string? Country { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public bool? IsActive { get; set; }
}