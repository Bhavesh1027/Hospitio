using FluentValidation;
namespace HospitioApi.Core.HandleProductModule.Queries.GetProductModules;
public class GetProductModulesValidator : AbstractValidator<GetProductModulesRequest>
{
    public GetProductModulesValidator()
    {
    }

    public class GetProductModulesInValidator : AbstractValidator<GetProductModulesIn>
    {
        public GetProductModulesInValidator()
        {
        }
    }
}
