using FluentValidation;
namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServiceById;
public class GetProductModuleServiceByIdValidator : AbstractValidator<GetProductModuleServiceByIdRequest>
{
    public GetProductModuleServiceByIdValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
    }
}
