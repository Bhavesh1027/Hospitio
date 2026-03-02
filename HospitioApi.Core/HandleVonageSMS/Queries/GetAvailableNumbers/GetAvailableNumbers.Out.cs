using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vonage.Numbers;
using static Vonage.Meetings.Common.Room;

namespace HospitioApi.Core.HandleVonageSMS.Queries.GetAvailableNumbers
{
    public class GetAvailableNumbersOut : BaseResponseOut
    {
        public GetAvailableNumbersOut(string message, string? jsonData) : base(message)
        {
            SmsList = jsonData;
        }

        public string? SmsList { get; set; }
    }
}