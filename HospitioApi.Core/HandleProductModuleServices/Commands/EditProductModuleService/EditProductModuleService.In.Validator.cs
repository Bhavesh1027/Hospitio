using FluentValidation;
namespace HospitioApi.Core.HandleProductModuleService.Commands.EditProductModuleService;
public class EditProductModuleServiceValidator : AbstractValidator<EditProductModuleServiceRequest>
{
    public EditProductModuleServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new EditProductModuleServiceInValidator());
        RuleFor(m => m.Id).NotNull().GreaterThan(0);
    }

    public class EditProductModuleServiceInValidator : AbstractValidator<EditProductModuleServiceIn>
    {
        public EditProductModuleServiceInValidator()
        {
            RuleFor(m => m.ProductId).GreaterThan(0);
            RuleFor(m => m.ProductModuleId).GreaterThan(0);
            RuleFor(m => m.ModuleServiceId).GreaterThan(0);
        }
    }
}
