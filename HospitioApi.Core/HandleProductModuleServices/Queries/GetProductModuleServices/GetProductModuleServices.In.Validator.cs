using FluentValidation;
namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServices;
public class GetProductModuleServicesValidator : AbstractValidator<GetProductModuleServicesRequest>
{
    public GetProductModuleServicesValidator()
    {
    }

    public class GetProductModuleServicesInValidator : AbstractValidator<GetProductModuleServicesIn>
    {
        public GetProductModuleServicesInValidator()
        {
        }
    }
}
