using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Commands.UploadCommunicationFile
{
    public class UploadCommunicationFileIn
    {
        public UploadCommunicationFileIn(IFormFile file, string documentType) 
        {
            File = file;
            DocumentType = documentType;
        }
        public IFormFile File { get; set; }
        public string DocumentType { get; set; }
   
    }
}
