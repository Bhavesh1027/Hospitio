using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerProduct;

public class UpdateCustomerProductValidator : AbstractValidator<UpdateCustomerProductRequest>
{
    public UpdateCustomerProductValidator()
    {
        RuleFor(m => m.In).SetValidator(new UpdateCustomerProductInValidation());
    }

    public class UpdateCustomerProductInValidation : AbstractValidator<UpdateCustomerProductIn>
    {
        public UpdateCustomerProductInValidation()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
            RuleFor(m => m.ProductId).NotEmpty().GreaterThan(0);
        }
    }
}
