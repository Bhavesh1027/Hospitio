using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public partial class ChannelUsers:Auditable
    {

        public int ChannelId { get; set; }

        public DateTime LastMessageReadTime { get; set; }


        public int? LastMessageReadId { get; set; }

        public virtual ChannelMessages? ChannelMessages { get; set; } 
     
        public virtual Channels? Channels { get; set; }
       
         
    }
}
