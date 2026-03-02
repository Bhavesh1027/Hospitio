using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitioApi.Core.HandleFiles.Queries.GetCommunicationFile
{
    public class GetCommunicationFileValidator :  AbstractValidator<GetCommunicationFileRequest>
    {
        public GetCommunicationFileValidator() 
        {
            //RuleFor(m => m.In).SetValidator(new GetCommunicationFileInValidator());
            //RuleFor(m => m.Location).NotEmpty();

        }

        public class GetCommunicationFileInValidator : AbstractValidator<GetCommunicationFileIn>
        {
            public GetCommunicationFileInValidator()
            {
                 //RuleFor(m => m.Location).NotEmpty();
            }
        }
    }
}
