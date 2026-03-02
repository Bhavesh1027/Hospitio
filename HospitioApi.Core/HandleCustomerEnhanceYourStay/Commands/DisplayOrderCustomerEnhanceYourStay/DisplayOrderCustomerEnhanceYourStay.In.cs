using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DisplayOrderCustomerEnhanceYourStay
{
    public class DisplayOrderCustomerEnhanceYourStayIn
    {
        public List<DisplayOrderCustomerEnhanceYourStay> displayOrderCustomerEnhanceYourStay { get; set; } = new List<DisplayOrderCustomerEnhanceYourStay> { };
    }
    public  class DisplayOrderCustomerEnhanceYourStay
    {
        public int? Id { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
