using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;

public class DeleteCustomerReceptionValidator : AbstractValidator<DeleteCustomerReceptionRequest>
{
    public DeleteCustomerReceptionValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerReceptionInValidator());
    }
    public class DeleteCustomerReceptionInValidator : AbstractValidator<DeleteCustomerReceptionIn>
    {
        public DeleteCustomerReceptionInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
