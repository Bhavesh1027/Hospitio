using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Commands.UpdateCustomerVonageNumber
{
    public class UpdateCustomerVonageNumberOut : BaseResponseOut
    {
        public UpdateCustomerVonageNumberOut(string message) : base(message)
        {
        }
    }
}
