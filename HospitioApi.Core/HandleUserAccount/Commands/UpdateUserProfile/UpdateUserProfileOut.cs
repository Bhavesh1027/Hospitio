using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile
{
    public class UpdateUserProfileOut:BaseResponseOut
    {
        public UpdateUserProfileOut(string message) : base(message)
        {
        }
    }
}
