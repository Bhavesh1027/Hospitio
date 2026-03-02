using HospitioApi.Shared;
using System.Text.Json.Serialization;

namespace HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomByCustomerRoomGuId;

public class GetCustomerRoomByCustomerRoomGuIdOut : BaseResponseOut
{
    public GetCustomerRoomByCustomerRoomGuIdOut(string message, CustomerRooms customerRoomNamesOuts) : base(message)
    {
        CustomerRoomNamesOut = customerRoomNamesOuts;
    }

    public CustomerRooms CustomerRoomNamesOut { get; set; } = new CustomerRooms();
}
public class CustomerRooms
{
    [JsonPropertyName("locationCode")]
    public Guid LocationCode { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("streetName")]
    public string StreetName { get; set; }
    [JsonPropertyName("streetNumber")]
    public string StreetNumber { get; set; }
    [JsonPropertyName("city")]
    public string City { get; set; }
    [JsonPropertyName("state")]
    public string State { get; set; }
    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; }
    [JsonPropertyName("country")]
    public string Country { get; set; }
    [JsonPropertyName("floor")]
    public string Floor { get; set; }
    [JsonPropertyName("apartmentNumber")]
    public string ApartmentNumber { get; set; }
}