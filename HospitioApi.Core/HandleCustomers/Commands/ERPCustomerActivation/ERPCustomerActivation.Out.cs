using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.ERPCustomerActivation
{
    public class ERPCustomerActivationOut : BaseResponseOut
    {
        public ERPCustomerActivationOut(string message, ERPCustomerstatusOut ERPCustomerStatusOut) : base(message)
        {
            ERPCustomerStatusOutput = ERPCustomerStatusOut;
        }
        public ERPCustomerstatusOut ERPCustomerStatusOutput { get; set; }
    }
    public class ERPCustomerstatusOut
    {
        public string? PylonUniqueCustomerId { get; set; }
        public bool? CustomerStatus { get;set; }

    }
}
