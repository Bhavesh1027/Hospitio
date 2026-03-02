using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public partial class ChannelUserTypeCustomerGuest: ChannelUsers
    {
       
        public int? UserId { get; set; }
    }
}   
