using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceById;

public class GetCustomerPropertyServiceByIdOut : BaseResponseOut
{
    public GetCustomerPropertyServiceByIdOut(string message, CustomerPropertyServiceByd customerPropertyService) : base(message)
    {
        CustomerPropertyService = customerPropertyService;
    }
    public CustomerPropertyServiceByd CustomerPropertyService { get; set; } = new CustomerPropertyServiceByd();
}

public class CustomerPropertyServiceByd
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Icon { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string ServiceImage { get; set; } = string.Empty;
}
