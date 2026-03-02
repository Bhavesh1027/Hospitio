using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DisplayOrderCustomerRoomService
{
    public class DisplayOrderCustomerRoomServiceIn
    {
        public List<DisplayOrderCustomerRoomService> DisplayOrderCustomerRoomService { get; set; } = new List<DisplayOrderCustomerRoomService>();
    }
    public class DisplayOrderCustomerRoomService
    {
        public int? Id { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
