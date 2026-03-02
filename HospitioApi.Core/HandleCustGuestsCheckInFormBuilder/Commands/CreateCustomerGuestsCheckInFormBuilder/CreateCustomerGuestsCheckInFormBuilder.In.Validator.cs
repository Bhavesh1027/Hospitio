using FluentValidation;


namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Commands.CreateCustomerGuestsCheckInFormBuilder;
public class CreateCustomerGuestsCheckInFormBuilderValidator : AbstractValidator<CreateCustomerGuestsCheckInFormBuilderRequest>
{
    public CreateCustomerGuestsCheckInFormBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new CustomerGuestsCheckInFormBuilderInValidator());
        RuleFor(m => m.In).NotNull();
    }
    public class CustomerGuestsCheckInFormBuilderInValidator : AbstractValidator<CreateCustomerGuestsCheckInFormBuilderIn>
    {
        public CustomerGuestsCheckInFormBuilderInValidator()
        {
            RuleFor(m => m.CustomerId).GreaterThan(0);
            RuleFor(m => m.SubmissionMail).NotEmpty().NotNull();
        }
    }
}
