using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.CreateCustomersPropertiesInfo;

public class CreateCustomersPropertiesInfoOut : BaseResponseOut
{
    public CreateCustomersPropertiesInfoOut(string message, CreatedCustomersPropertiesInfoOut createdCustomersPropertiesInfoOut) : base(message)
    {
        CreatedCustomersPropertiesInfoOut = createdCustomersPropertiesInfoOut;
    }
    public CreatedCustomersPropertiesInfoOut CreatedCustomersPropertiesInfoOut { get; set; }
}
public class CreatedCustomersPropertiesInfoOut
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
    public bool? IsActive { get; set; }
}