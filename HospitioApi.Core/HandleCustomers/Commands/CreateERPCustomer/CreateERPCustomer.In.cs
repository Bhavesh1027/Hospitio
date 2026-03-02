namespace HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;

public class CreateERPCustomerIn
{
    public string? PylonUniqueCustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? CompanyName { get; set; }
    public string? BusinessType { get; set; }
    public int? NoOfRooms { get; set; }
    public string? PhoneCountry { get; set; }
    public string? Mobile { get; set; }
    public string? ServicePack { get; set; }
    public int ExpirationInDay { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; } = null;
    //public bool? IsCenturionCustomer { get; set; }
    //public string? VAT { get; set; }
    //public string? StreetAddress { get; set; }
    //public string? StreetNumber { get; set; }
    //public string? PostalCode { get; set; }
    //public string? Country { get; set; }
    //public string? Province { get; set; }
    //public string? PurchaseType { get; set; } // Service/ Upgrade/ Renewal
}
