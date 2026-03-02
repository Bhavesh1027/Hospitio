using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Queries.GetCommunicationFile
{
    public class GetCommunicationFileOut : BaseResponseOut
    {
        public GetCommunicationFileOut(string message, MemoryStream memoryStream,
        string fileDownloadName, string contentType) : base(message)
        {
            MemoryStream = memoryStream;
            FileDownloadName = fileDownloadName;
            ContentType = contentType;
        }
        public MemoryStream MemoryStream { get; }
        public string FileDownloadName { get; }
        public string ContentType { get; }
    }

    public class GetCommunicationFileOutV1 : BaseResponseOut
    {
        public GetCommunicationFileOutV1(string message,
        string fileDownloadName, string contentType, string tempSasUri, DateTime expireAt) : base(message)
        {
            FileDownloadName = fileDownloadName;
            ContentType = contentType;
            TempSasUri = tempSasUri;
            ExpireAt = expireAt;
        }
        public string FileDownloadName { get; }
        public string ContentType { get; }
        public string TempSasUri { get; set; }
        public DateTime ExpireAt { get; set; }

    }
}
