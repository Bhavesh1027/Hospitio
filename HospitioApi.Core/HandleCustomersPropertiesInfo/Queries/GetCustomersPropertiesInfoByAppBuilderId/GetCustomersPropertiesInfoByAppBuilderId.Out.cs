using HospitioApi.Shared;
using System.Collections.Specialized;

namespace HospitioApi.Core.HandleCustomersPropertiesInfo.Queries.GetCustomersPropertiesInfoByAppBuilderId;

public class GetCustomersPropertiesInfoByAppBuilderIdOut : BaseResponseOut
{
    public GetCustomersPropertiesInfoByAppBuilderIdOut(string message, List<CustomersPropertiesInfoByAppBuilderIdOut> customersPropertiesInfoByAppBuilderIdOut) : base(message)
    {
        CustomersPropertiesInfoByAppBuilderIdOut = customersPropertiesInfoByAppBuilderIdOut;
    }
    public List<CustomersPropertiesInfoByAppBuilderIdOut> CustomersPropertiesInfoByAppBuilderIdOut { get; set; } = new();
}
public class CustomersPropertiesInfoByAppBuilderIdOut
{
    public int? Id { get; set; }
    public int CustomerGuestAppBuilderId { get; set; }
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
    public string? RoomName { get; set; }
    public string? BusinessType { get; set; }
    public List<CustomerPropertyInfoServicesOut> CustomerPropertyInfoServicesOuts { get; set; } = new List<CustomerPropertyInfoServicesOut>();
    public List<CustomerPropertyInfoGalleriesOut> CustomerPropertyInfoGalleriesOuts { get; set; } = new();
    public List<CustomerPropertyInfoEmergencyNumbersOut> CustomerPropertyInfoEmergencyNumbersOuts { get; set; } = new List<CustomerPropertyInfoEmergencyNumbersOut>();
    public List<CustomerPropertyInfoExtrasOut> CustomerPropertyInfoExtrasOuts { get; set; } = new List<CustomerPropertyInfoExtrasOut>();
}
public class CustomerPropertyInfoServicesOut
{
    public int Id { get; set; }
    public int CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public List<CustomerPropertyInfoServiceImagesOut> CustomerPropertyInfoServiceImagesOuts { get; set; } = new List<CustomerPropertyInfoServiceImagesOut>();
}
public class CustomerPropertyInfoServiceImagesOut
{
    public int Id { get; set; }
    public int CustomerPropertyServiceId { get; set; }
    public string? ServiceImages { get; set; }
}

public class CustomerPropertyInfoGalleriesOut
{
    public int Id { get; set; }
    public int CustomerPropertyInformationId { get; set; }
    public string? PropertyImage { get; set; }
}
public class CustomerPropertyInfoEmergencyNumbersOut
{
    public int Id { get; set; }
    public int CustomerPropertyInformationId { get; set; }
    public string? Name { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public int? DisplayOrder { get; set; }
}
public class CustomerPropertyInfoExtrasOut
{
    public int Id { get; set; }
    public int CustomerPropertyInformationId { get; set; }
    public int? ExtraType { get; set; }
    public string? Name { get; set; }
    public int? DisplayOrder { get; set; }
    public List<CustomerPropertyInfoExtraDetailsOut> customerPropertyExtraDetailsOuts { get; set; } = new List<CustomerPropertyInfoExtraDetailsOut>();
}
public class CustomerPropertyInfoExtraDetailsOut
{
    public int Id { get; set; }
    public int CustomerPropertyExtraId { get; set; }
    public string? Description { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
}