using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerAppBuilder;

public class DeleteCustomerGuestAppBuilderValidator : AbstractValidator<DeleteCustomerGuestAppBuilderRequest>
{
    public DeleteCustomerGuestAppBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerGuestAppBuilderInValidator());
    }
    public class DeleteCustomerGuestAppBuilderInValidator : AbstractValidator<DeleteCustomerGuestAppBuilderIn>
    {
        public DeleteCustomerGuestAppBuilderInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
