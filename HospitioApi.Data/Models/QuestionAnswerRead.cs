using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Data.Models
{
    public class QuestionAnswerRead : Auditable
    {
        public int QuestionAnswerId { get; set; }
        public int UserId { get; set; }
    }
}
