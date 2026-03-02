using FluentValidation;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Queries.GetCustomerHouseKeepingWithRelation;

public class GetCustomerHouseKeepingWithRelationValidator : AbstractValidator<GetCustomerHouseKeepingWithRelationRequest>
{
    public GetCustomerHouseKeepingWithRelationValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerHouseKeepingWithRelationInValidator());

    }
    public class GetCustomerHouseKeepingWithRelationInValidator : AbstractValidator<GetCustomerHouseKeepingWithRelationIn>
    {
        public GetCustomerHouseKeepingWithRelationInValidator()
        {             
            RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
