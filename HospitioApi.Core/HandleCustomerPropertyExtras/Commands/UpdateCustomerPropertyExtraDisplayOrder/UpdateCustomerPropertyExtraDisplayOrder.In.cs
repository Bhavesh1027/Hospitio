using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.NewFolder
{

    public class UpdatedCustomerPropertyExtraDisplayOrderIn
    {
        public List<UpdateCustomerPropertyExtraDisplayOrderIn> updateCustomerPropertyExtraDisplayOrderIns { get; set; } = new();
    }
    public class UpdateCustomerPropertyExtraDisplayOrderIn
    {
        public int? Id { get; set; }

        public int? DisplayOrder { get; set; }

        public byte? ExtraType { get; set; }

    }
}
