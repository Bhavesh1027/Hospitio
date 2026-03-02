using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequestStatus
{
    public class UpdateGuestRequestStatusOut : BaseResponseOut
    {
        public UpdateGuestRequestStatusOut(string message) : base(message)
        {
        }
    }
}
