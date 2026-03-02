using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerAppBuilder;

public class UpdateCustomerGuestAppBuilderValidator : AbstractValidator<UpdateCustomerGuestAppBuilderRequest>
{
    public UpdateCustomerGuestAppBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerGuestAppBuilderInValidator());
    }

    public class UpdateCustomerGuestAppBuilderInValidator : AbstractValidator<UpdateCustomerGuestAppBuilderIn>
    {
        public UpdateCustomerGuestAppBuilderInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.CustomerRoomNameId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.IsActive).NotEmpty().NotNull().Must(x => x == false || x == true);
        }
    }
}
