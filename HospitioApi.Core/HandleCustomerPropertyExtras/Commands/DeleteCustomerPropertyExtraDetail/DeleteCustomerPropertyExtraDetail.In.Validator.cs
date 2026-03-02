using FluentValidation;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;

public class DeleteCustomerPropertyExtraDetailValidator : AbstractValidator<DeleteCustomerPropertyExtraDetailRequest>
{
    public DeleteCustomerPropertyExtraDetailValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerPropertyExtraDetailInValidator());
    }
    public class DeleteCustomerPropertyExtraDetailInValidator : AbstractValidator<DeleteCustomerPropertyExtraDetailIn>
    {
        public DeleteCustomerPropertyExtraDetailInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
