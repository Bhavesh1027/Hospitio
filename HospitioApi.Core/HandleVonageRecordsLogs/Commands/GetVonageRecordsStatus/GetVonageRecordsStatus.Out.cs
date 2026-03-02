using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Shared;
using System.Diagnostics.Contracts;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.GetVonageRecordsStatus
{
    public class GetVonageRecordsStatusOut : BaseResponseOut
    {
        public GetVonageRecordsStatusOut(string message) : base(message) 
        {
         
        }
        public WebFileOut csvContent { get; set; }
    }
    
}
