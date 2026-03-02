using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers
{
    public class GetAvailableNumbersIn
    {
        public int? CustomerId { get; set; }
        public string? country { get; set; }
        public string type { get; set; }
        public string? pattern { get; set; }
        public int search_pattern { get; set; }
        public string? features { get; set; }
        public int? size { get; set; }
        public int? index { get; set; }
    }
}
