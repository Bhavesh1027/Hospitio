using FluentValidation;
using HospitioApi.Core.HandleLeads.Queries.GetLeadById;

namespace HospitioApi.Core.HandleLeads.Queries.GetLeadByIdl;

public class GetLeadByIdValidator : AbstractValidator<GetLeadByIdRequest>
{
    public GetLeadByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetLeadByIdInValidator());
    }
    public class GetLeadByIdInValidator : AbstractValidator<GetLeadByIdIn>
    {
        public GetLeadByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
