using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropeetyEmergencyNumberDisplayOrder
{
    public class UpdatedCustomerPropeetyEmergencyNumberDisplayOrderIn
    {
        public List<UpdateCustomerPropeetyEmergencyNumberDisplayOrderIn> UpdateCustomerPropeetyEmergencyNumberDisplayorderIn { get; set; } = new();
    }
    public class UpdateCustomerPropeetyEmergencyNumberDisplayOrderIn
    {
        public int? Id { get; set; }
        public int? DisplayOrder { get; set; }
    }

}
