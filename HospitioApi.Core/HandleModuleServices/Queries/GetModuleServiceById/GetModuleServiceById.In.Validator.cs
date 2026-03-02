using FluentValidation;

namespace HospitioApi.Core.HandleModuleServices.Queries.GetModuleServiceById;

public class GetModuleServiceByIdValidator : AbstractValidator<GetModuleServiceByIdRequest>
{
    public GetModuleServiceByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetModuleServiceInValidator());
    }
    public class GetModuleServiceInValidator : AbstractValidator<GetModuleServiceByIdIn>
    {
        public GetModuleServiceInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
