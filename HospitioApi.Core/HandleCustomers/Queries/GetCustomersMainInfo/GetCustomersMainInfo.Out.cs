using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;

public class GetCustomersMainInfoOut : BaseResponseOut
{
    public GetCustomersMainInfoOut(string message, List<CustomersMainInfoOut> customersMainInfoOut) : base(message)
    {
        CustomersMainInfoOut = customersMainInfoOut;
    }
    public List<CustomersMainInfoOut> CustomersMainInfoOut { get; set; } = new List<CustomersMainInfoOut>();
}
public class CustomersMainInfoOut
{
    public int Id { get; set; }
    public string? BusinessName { get; set; }
    public string? ServicePackName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BizType { get; set; }
    public string? Title { get; set; }
    public string? ProfilePicture { get; set; }
    public int? FilteredCount { get; set; }
}
