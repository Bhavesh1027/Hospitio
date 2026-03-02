using FluentValidation;


namespace HospitioApi.Core.HandleFiles.Commands.CreateFile;

public class CreateFileValidator : AbstractValidator<CreateFileRequest>
{
    public CreateFileValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateFileInValidator());
    }

    public class CreateFileInValidator : AbstractValidator<CreateFileIn>
    {
        public CreateFileInValidator()
        {
            RuleFor(m => m.File).NotNull();
            //RuleFor(m => m.DocumentType).IsInEnum().WithMessage("Invalid Document Type.");
            //RuleFor(m => m.UserId).NotEmpty();
        }
    }
}