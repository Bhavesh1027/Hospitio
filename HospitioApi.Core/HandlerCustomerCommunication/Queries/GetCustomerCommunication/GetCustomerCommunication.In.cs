
namespace HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunication
{
    public class GetCustomerCommunicationIn
    {
        public int CustomerId { get; set; }
        public string SearchString { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public string? UserLevel { get; set; }
        public int? CustomerUserId { get; set; }
    }
}
