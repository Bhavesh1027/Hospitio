using FluentValidation;
namespace HospitioApi.Core.HandleProductModule.Commands.EditProductModule;
public class EditProductModuleValidator : AbstractValidator<EditProductModuleRequest>
{
    public EditProductModuleValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditProductModuleInValidator());
        RuleFor(m => m.Id).NotNull().GreaterThan(0);
    }

    public class EditProductModuleInValidator : AbstractValidator<EditProductModuleIn>
    {
        public EditProductModuleInValidator()
        {
            RuleFor(m => m.ProductId).GreaterThan(0);
            RuleFor(m => m.ModuleId).GreaterThan(0);
            RuleFor(m => m.Price).GreaterThan(0);
        }
    }
}
