using FluentValidation;
namespace HospitioApi.Core.HandleProduct.Commands.CreateProduct;
public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateProductInValidator());
    }

    public class CreateProductInValidator : AbstractValidator<CreateProductIn>
    {
        public CreateProductInValidator()
        {
            RuleFor(m => m.ProductName).NotEmpty();
            RuleForEach(m => m.ProductModules).ChildRules(p =>
            {
                p.RuleFor(m => m.ModuleId).NotEmpty().GreaterThan(0);
                p.RuleFor(m => m.Currency).MaximumLength(3);
                p.RuleForEach(m => m.ProductModuleServices).ChildRules(ps =>
                {
                    ps.RuleFor(m => m.ModuleServiceId).NotEmpty().GreaterThan(0);
                });
            });
        }
    }
}
