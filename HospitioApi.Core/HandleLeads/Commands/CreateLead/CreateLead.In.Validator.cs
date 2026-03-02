using FluentValidation;


namespace HospitioApi.Core.HandleLeads.Commands.CreateLead;


public class CreateLeadValidator : AbstractValidator<CreateLeadRequest>
{
    public CreateLeadValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateLeadInValidator());
    }

    public class CreateLeadInValidator : AbstractValidator<CreateLeadIn>
    {
        public CreateLeadInValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Company).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();
            RuleFor(m => m.Comment).NotEmpty();
        }
    }
}