using FluentValidation;


namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.EditCustomerGuestsCheckInFormBuilder;
public class EditCustomerGuestsCheckInFormBuilderValidator : AbstractValidator<EditCustomerGuestsCheckInFormBuilderRequest>
{
    public EditCustomerGuestsCheckInFormBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditCustomerGuestsCheckInFormBuilderInValidator());
    }
    public class EditCustomerGuestsCheckInFormBuilderInValidator : AbstractValidator<EditCustomerGuestsCheckInFormBuilderIn>
    {
        public EditCustomerGuestsCheckInFormBuilderInValidator()
        {
            RuleFor(m => m.CustomerId).GreaterThan(0);
            RuleFor(m => m.Id).GreaterThan(0);
            RuleFor(m => m.SubmissionMail).NotEmpty().NotNull();
        }
    }
}
