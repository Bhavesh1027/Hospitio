using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.Services.Vonage.Models
{
    public class VonageButton
    {
        public string type { get; set; }
        public string text { get; set; }
        public string? url { get; set; }
        public string? phone_number { get; set; }

        // Optional: Add a custom validation method to ensure only one property is set
        public bool IsValid()
        {
            return (url != null && phone_number == null) || (url == null && phone_number != null);
        }
    }
}
