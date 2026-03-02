namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerUser;

public class UpdateCustomerUserIn
{
	public int CustomerId { get; set; }
	public string UserName { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? EmailAddress { get; set; }
	public string? PhoneCountry { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Country { get; set; }
	public string? IncomingTranslationLangage { get; set; }
	public string? Password { get; set; }
	public bool? IsActive { get; set; }
	public DateTime? SubscriptionExpirationDate { get; set; }

}
