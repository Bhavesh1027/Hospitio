using FluentValidation;
namespace HospitioApi.Core.HandleProductModuleService.Commands.CreateProductModuleService;
public class CreateProductModuleServiceValidator : AbstractValidator<CreateProductModuleServiceRequest>
{
    public CreateProductModuleServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateProductModuleServiceInValidator());
    }

    public class CreateProductModuleServiceInValidator : AbstractValidator<CreateProductModuleServiceIn>
    {
        public CreateProductModuleServiceInValidator()
        {
            RuleFor(m => m.ProductId).GreaterThan(0);
            RuleFor(m => m.ProductModuleId).GreaterThan(0);
            RuleFor(m => m.ModuleServiceId).GreaterThan(0);
        }
    }
}
