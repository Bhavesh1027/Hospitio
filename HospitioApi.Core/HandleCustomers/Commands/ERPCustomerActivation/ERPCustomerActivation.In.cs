using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomers.Commands.ERPCustomerActivation
{
    public class ERPCustomerActivationIn
    {
        public string? PylonUniqueCustomerId { get; set; }
        public string? CustomerStatus { get; set; }
    }
}
