using FluentValidation;

namespace HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;

public class GetCustomerConciergeWithRelationValidator : AbstractValidator<GetCustomerConciergeWithRelationRequest>
{
    public GetCustomerConciergeWithRelationValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerConciergeWithRelationInValidator());

    }
    public class GetCustomerConciergeWithRelationInValidator : AbstractValidator<GetCustomerConciergeWithRelationIn>
    {
        public GetCustomerConciergeWithRelationInValidator()
        {          
            RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
