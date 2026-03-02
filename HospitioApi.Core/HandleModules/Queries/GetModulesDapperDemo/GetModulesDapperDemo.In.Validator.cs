using FluentValidation;

namespace HospitioApi.Core.HandleModules.Queries.GetModulesDapperDemo;

public class GetModulesDapperDemoValidator : AbstractValidator<GetModulesDapperDemoRequest>
{
    public GetModulesDapperDemoValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetModulesDapperDemoValidatorInValidator());

    }
    public class GetModulesDapperDemoValidatorInValidator : AbstractValidator<GetModulesDapperDemoIn>
    {
        public GetModulesDapperDemoValidatorInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }

}
