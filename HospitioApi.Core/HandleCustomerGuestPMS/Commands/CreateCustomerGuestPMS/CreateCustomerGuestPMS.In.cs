using Microsoft.AspNetCore.Http;

namespace HospitioApi.Core.HandleCustomerGuestPMS.Commands.CreateCustomerGuestPMS;

public class CreateCustomerGuestPMSIn
{
    public CreateCustomerGuestPMSIn() { }
    public IFormFile DocumentAttachment { get; set; }
    public string? DocumentType { get; set; }
    public string? ContainerName { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? MobileNumber { get; set; }   
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? ReservationNumber { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? VATNumber { get; set; }
    public string? DocumentName { get; set; }
    //public string? DocumentAttachment { get; set; }
}
