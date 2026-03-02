namespace HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;

public class CreateCustomerIn
{
    public string? BusinessName { get; set; }
    public int? BusinessTypeId { get; set; }
    public int? NoOfRooms { get; set; }
    public string? PhoneCountry { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
    public int? ProductId { get; set; }
    public string? CenturianHotelCode { get; set; }
    public string? WatsappCountry { get; set; }
    public string? WatsappNumber { get; set; }
    public string? ViberCountry { get; set; }
    public string? ViberNumber { get; set; }
    public string? TelegramCounty { get; set; }
    public string? TelegramNumber { get; set; }
    public string? VatNumber { get; set; }
    public CustomerUserIn CustomerUserIn { get; set; } = new();
    //public ChannelAddIn ChannelAddIn { get; set; } = new();

}

public class CustomerUserIn
{
    public bool? IsActive { get; set; }
    public int? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Title { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }

}

