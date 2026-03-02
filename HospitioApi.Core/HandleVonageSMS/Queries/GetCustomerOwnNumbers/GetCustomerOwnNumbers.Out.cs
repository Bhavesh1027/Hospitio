using HospitioApi.Shared;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers
{
    public class GetCustomerOwnNumbersOut:BaseResponseOut
    {
        public GetCustomerOwnNumbersOut(string message, string? JsonData) :base(message)
        {
            SmsList = JsonData;
        }
        public string? SmsList { get; set; }
    }
}
