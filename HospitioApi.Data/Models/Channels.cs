using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public partial class Channels : Auditable
    {
        public string? Uuid { get; set; }

        public string? CreateForm { get; set; }
        public int? channelUserID { get; set; } 
        public virtual ICollection<ChannelUsers>? ChannelUsers { get; set; }
        public virtual ICollection<ChannelMessages>? ChannelMessages { get; set; }

    }
}
