using FluentValidation;
namespace HospitioApi.Core.HandleProductModule.Commands.CreateProductModule;
public class CreateProductModuleValidator : AbstractValidator<CreateProductModuleRequest>
{
    public CreateProductModuleValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateProductModuleInValidator());
    }

    public class CreateProductModuleInValidator : AbstractValidator<CreateProductModuleIn>
    {
        public CreateProductModuleInValidator()
        {
            RuleFor(m => m.ProductId).GreaterThan(0);
            RuleFor(m => m.ModuleId).GreaterThan(0);
            RuleFor(m => m.Price).GreaterThan(0);
        }
    }
}
