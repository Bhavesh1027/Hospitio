using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;

public class CreateCustomerGuestAppBuilderValidator : AbstractValidator<CreateCustomerGuestAppBuilderRequest>
{
    public CreateCustomerGuestAppBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateCustomerGuestAppBuilderInValidator());
    }
    public class CreateCustomerGuestAppBuilderInValidator : AbstractValidator<CreateCustomerGuestAppBuilderIn>
    {
        public CreateCustomerGuestAppBuilderInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.CustomerRoomNameId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.IsActive).NotEmpty().NotNull().Must(x => x == false || x == true);
        }
    }
}
