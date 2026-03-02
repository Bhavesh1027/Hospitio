using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReception.Queries.GetCustomerReceptionWithRelation;

public class GetCustomerReceptionWithRelationValidator : AbstractValidator<GetCustomerReceptionWithRelationRequest>
{
    public GetCustomerReceptionWithRelationValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerReceptionWithRelationInValidator());

    }
    public class GetCustomerReceptionWithRelationInValidator : AbstractValidator<GetCustomerReceptionWithRelationIn>
    {
        public GetCustomerReceptionWithRelationInValidator()
        {            
            RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
