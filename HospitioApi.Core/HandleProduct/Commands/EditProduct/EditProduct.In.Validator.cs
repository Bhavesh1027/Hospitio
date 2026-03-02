using FluentValidation;
namespace HospitioApi.Core.HandleProduct.Commands.EditProduct;
public class EditProductValidator : AbstractValidator<EditProductRequest>
{
    public EditProductValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditProductInValidator());
        RuleFor(m => m.Id).NotNull().GreaterThan(0);
    }

    public class EditProductInValidator : AbstractValidator<EditProductIn>
    {
        public EditProductInValidator()
        {
            RuleFor(m => m.Name).NotEmpty();
        }
    }
}
