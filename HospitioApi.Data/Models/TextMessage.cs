using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public  class TextMessage:ChannelMessages
    {
        public string? Message { get; set; }

        public string? TranslateMessage { get; set; }
    }
}
