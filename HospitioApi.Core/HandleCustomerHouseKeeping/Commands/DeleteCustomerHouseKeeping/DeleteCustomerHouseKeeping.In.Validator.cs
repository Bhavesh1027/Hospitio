using FluentValidation;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;

public class DeleteCustomerHouseKeepingValidator : AbstractValidator<DeleteCustomerHouseKeepingRequest>
{
    public DeleteCustomerHouseKeepingValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerHouseKeepingInValidator());
    }
    public class DeleteCustomerHouseKeepingInValidator : AbstractValidator<DeleteCustomerHouseKeepingIn>
    {
        public DeleteCustomerHouseKeepingInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
