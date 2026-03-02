using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping
{
    public class DisplayOrderCustomerHouseKeepingIn
    {
        public List<DisplayOrderCustomerHouseKeeping> displayOrderCustomerHouseKeepings { get; set; }=new List<DisplayOrderCustomerHouseKeeping>(); 
    }

    public class DisplayOrderCustomerHouseKeeping
    {
        public int? Id { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
