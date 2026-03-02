using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Vonage.Models
{
    public class VonageCustom
    {
        public string type { get; set; }
        public VonageTemplate template { get; set; }
    }
}
