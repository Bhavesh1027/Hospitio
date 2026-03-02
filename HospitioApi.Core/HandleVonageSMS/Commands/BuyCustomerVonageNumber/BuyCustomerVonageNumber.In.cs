using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber
{
    public class BuyCustomerVonageNumberIn
    {
        public int CustomerId { get; set; }
        public string Country { get; set; }
        public string Number { get; set; } 
    }
}
