using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestCheckInFormBuilderIcon
{
    public class GetCustomerGuestCheckInFormBuilderIconOut : BaseResponseOut
    {
        public GetCustomerGuestCheckInFormBuilderIconOut(string message, CustomerGuestCheckInFormBuilderIconOut CustomerGuestCheckInFormBuilderIconOut) : base(message)
        {
            customerGuestCheckInFormBuilderIconOut = CustomerGuestCheckInFormBuilderIconOut;
        }
        public CustomerGuestCheckInFormBuilderIconOut customerGuestCheckInFormBuilderIconOut { get; set; }
    }

    public class CustomerGuestCheckInFormBuilderIconOut
    {
        public string? JsonData { get; set; }
    }
}
