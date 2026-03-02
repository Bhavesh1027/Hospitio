using HospitioApi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Commands.UploadCommunicationFile
{
    public class UploadCommunicationFileOut:BaseResponseOut
    {
        public UploadCommunicationFileOut(
        string message, MemoryStream memoryStream, string fileName,
        string location, string contentType, string tempSasUri) : base(message)
        {
            MemoryStream = memoryStream;
            Location = location;
            ContentType = contentType;
            FileName = fileName;
            TempSasUri = tempSasUri;
        }

        public MemoryStream MemoryStream { get; set; }
        public string FileName { get; set; }
        public string Location { get; }
        public string ContentType { get; }
        public string TempSasUri { get; }
    }

    public class UploadCommunicationFileOutV1 : BaseResponseOut
    {
        public UploadCommunicationFileOutV1(
           string message, string fileName,
           string location, string contentType, string tempSasUri, DateTime expireAt) : base(message)
        {
            FileName = fileName;
            Location = location;
            ContentType = contentType;
            TempSasUri = tempSasUri;
            ExpireAt = expireAt;
        }

        public string FileName { get; set; }
        public string Location { get; set; }
        public string ContentType { get; set; }
        public string TempSasUri { get; set; }
        public DateTime ExpireAt { get; set; }

    }
}
