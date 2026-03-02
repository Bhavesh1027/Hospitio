using FluentValidation;
using HospitioApi.Core.HandleFiles.Commands.CreateFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HospitioApi.Core.HandleFiles.Commands.CreateFile.CreateFileValidator;

namespace HospitioApi.Core.HandleFiles.Commands.UploadCommunicationFile
{
    public class UploadCommunicationFileValidator : AbstractValidator<UploadCommunicationFileRequest>
    {
        public UploadCommunicationFileValidator() 
        {
            RuleFor(m => m.In).SetValidator(new UploadCommunicationFileInValidator());
        }

        public class UploadCommunicationFileInValidator : AbstractValidator<UploadCommunicationFileIn>
        {
            public UploadCommunicationFileInValidator()
            {
                RuleFor(m => m.File).NotNull();
                //RuleFor(m => m.DocumentType).IsInEnum().WithMessage("Invalid Document Type.");
               
            }
        }
    }
   
}