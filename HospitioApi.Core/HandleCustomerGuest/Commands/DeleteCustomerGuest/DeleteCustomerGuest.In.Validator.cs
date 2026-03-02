using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;

public class DeleteCustomerGuestValidator : AbstractValidator<DeleteCustomerGuestRequest>
{
    public DeleteCustomerGuestValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerGuestInValidator());
    }
    public class DeleteCustomerGuestInValidator : AbstractValidator<DeleteCustomerGuestIn>
    {
        public DeleteCustomerGuestInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
