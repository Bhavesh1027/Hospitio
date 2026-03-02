using FluentValidation;
namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModuleById;
public class GetProductModuleByIdValidator : AbstractValidator<GetProductModuleByIdRequest>
{
    public GetProductModuleByIdValidator()
    {
        RuleFor(m => m.Id).GreaterThan(0);
    }
}
