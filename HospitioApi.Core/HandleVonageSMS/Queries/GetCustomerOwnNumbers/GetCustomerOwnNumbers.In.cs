using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetCustomerOwnNumbers
{
    public class GetCustomerOwnNumbersIn
    {
        public int? CustomerId { get; set; }
        public int? search_pattern { get; set; }
        public string? pattern { get; set; }
    }
}
