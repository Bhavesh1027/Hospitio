using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;

public class GetCustomerGuestAppBuilderByCustomerRoomIdValidator : AbstractValidator<GetCustomerGuestAppBuilderByCustomerRoomIdRequest>
{
    public GetCustomerGuestAppBuilderByCustomerRoomIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestAppBuilderByCustomerRoomIdInValidator());
    }
    public class GetCustomerGuestAppBuilderByCustomerRoomIdInValidator : AbstractValidator<GetCustomerGuestAppBuilderByCustomerRoomIdIn>
    {
        public GetCustomerGuestAppBuilderByCustomerRoomIdInValidator()
        {
            RuleFor(m => m.RoomId).NotEmpty().GreaterThan(0);
        }
    }
}
