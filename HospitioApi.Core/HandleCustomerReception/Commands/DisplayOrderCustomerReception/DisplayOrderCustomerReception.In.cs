namespace HospitioApi.Core.HandleCustomerReception.Commands.DisplayCustomerReception;

public class DisplayOrderCustomerReceptionIn
{
    public List<DisplayOrderCustomerReception> DisplayOrderCustomerReception { get; set; } = new List<DisplayOrderCustomerReception>();
}

public class DisplayOrderCustomerReception
{
    public int? Id { get; set; }
    public int? DisplayOrder { get;set; }
}