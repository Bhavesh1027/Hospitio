using FluentValidation;


namespace HospitioApi.Core.HandleLeads.Commands.DeleteLead;
public class DeleteLeadValidator : AbstractValidator<DeleteLeadRequest>
{
    public DeleteLeadValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteLeadInValidator());
    }

    public class DeleteLeadInValidator : AbstractValidator<DeleteLeadIn>
    {
        public DeleteLeadInValidator()
        {
            RuleFor(m => m.LeadId).NotEmpty().GreaterThan(0);
        }
    }
}