namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersByGuIds;

public class GetCustomersByGuIdsIn
{
    public List<Guid> guids { get; set; } = new();
}
