using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vonage.Numbers;

namespace HospitioApi.Core.HandleVonageSMS.Commands.BuyCustomerVonageNumber
{

    public class BuyCustomerVonageNumberOut : BaseResponseOut
    {
        public BuyCustomerVonageNumberOut(string message) : base(message)
        {
           
        }

    }
}
