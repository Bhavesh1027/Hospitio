namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;

public class DisplayOrderCustomerConcierageIn
{
    public  List<DisplayOrderCustomerConcierage> DisplayOrderCustomerConcierage { get; set; } = new List<DisplayOrderCustomerConcierage>();
}
public class DisplayOrderCustomerConcierage
{
    public int? Id { get; set; }
    public int? DisplayOrder { get; set; }
}