using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequestStatus
{
    public class UpdateGuestRequestStatusIn
    {
        public int? GuestRequestId { get; set; }
        public int? EnhanceStayGuestRequestId { get; set; }
        public GuestRequestStatusEnum Status { get; set; }
    }
}
