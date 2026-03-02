using FluentValidation;


namespace HospitioApi.Core.HandleLeads.Commands.EditLead;
public class EditLeadValidator : AbstractValidator<EditLeadRequest>
{
    public EditLeadValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditLeadInValidator());
    }

    public class EditLeadInValidator : AbstractValidator<EditLeadIn>
    {
        public EditLeadInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();
            RuleFor(m => m.Company).NotEmpty();
            RuleFor(m => m.Comment).NotEmpty();
            RuleFor(m => m.PhoneCountry).NotEmpty().MaximumLength(3);
        }
    }
}