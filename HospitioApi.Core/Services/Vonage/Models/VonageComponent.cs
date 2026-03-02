using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Vonage.Models
{
    public class VonageComponent
    {
        public string type { get; set; }
        public List<VonageParameter> parameters { get; set; }
        public string index { get; set; }
        public string sub_type { get; set; }
    }
}
