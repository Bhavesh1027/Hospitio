using FluentValidation;
using HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerEnhanceYourStayCategoryWithRelation;

namespace HospitioApi.Core.HandleCustomerRoomService.Queries.GetCustomerRoomServiceWithRelation;

public class GetCustomerRoomServiceWithRelationValidator : AbstractValidator<GetCustomerRoomServiceWithRelationRequest>
{
    public GetCustomerRoomServiceWithRelationValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerRoomServiceWithRelationInValidator());

    }
    public class GetCustomerRoomServiceWithRelationInValidator : AbstractValidator<GetCustomerRoomServiceWithRelationIn>
    {
        public GetCustomerRoomServiceWithRelationInValidator()
        {
            RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);
        }
    }
}
