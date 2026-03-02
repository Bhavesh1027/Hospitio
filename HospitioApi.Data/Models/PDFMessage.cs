using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{

    public partial class PdfMessage : ChannelMessages
    {
        public string? Url { get; set; }
    }
   
}
