using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public class VonageCredentials : Auditable
    {
        public int? CustomerId { get; set; }
        public string? SubAccountName { get; set; }
        public string? APIKey { get; set; } 
        public string? APISecret { get; set; } 
        public string? AppId { get; set; } 
        public string? AppPrivatKey { get; set; }
        public string? AppPublicKey { get; set; }
        public string? WABAId { get; set;}
    }
}
