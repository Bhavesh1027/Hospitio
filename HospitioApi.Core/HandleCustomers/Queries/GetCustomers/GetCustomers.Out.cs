using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomers
{
    public class GetCustomersOut : BaseResponseOut
    {
        public GetCustomersOut(string message, List<CustomerDetails> Customers) : base(message) 
        {
            this.Customers =  Customers;
        }
        public List<CustomerDetails> Customers { get; set; }
    }
    public class CustomerDetails
    {
        public int Id { get; set;}
        public string CustomerName { get; set;}
    }
}
