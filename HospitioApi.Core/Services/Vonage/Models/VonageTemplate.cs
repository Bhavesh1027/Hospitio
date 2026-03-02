using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Vonage.Models
{
    public class VonageTemplate
    {
        //public string @namespace { get; set; }
        public string name { get; set; }
        public VonageLanguage language { get; set; }
        public List<VonageComponent> components { get; set; }
    }
}
