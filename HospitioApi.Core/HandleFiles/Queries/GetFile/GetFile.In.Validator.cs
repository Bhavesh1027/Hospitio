using FluentValidation;

namespace HospitioApi.Core.HandleFiles.Queries.GetFile;


public class GetFileRequestValidator : AbstractValidator<GetFileRequest>
{
    public GetFileRequestValidator()
    {
        //RuleFor(m => m.In).SetValidator(new GetFileRequestInValidator());
    }

    public class GetFileRequestInValidator : AbstractValidator<GetFileIn>
    {
        public GetFileRequestInValidator()
        {
            // RuleFor(m => m.Location).NotEmpty();
        }
    }
}