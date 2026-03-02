using FluentValidation;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.CreateGuestRequest;

public class CreateGuestRequestValidator : AbstractValidator<CreateGuestRequestRequest>
{
    public CreateGuestRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGuestRequestInValidator());
    }
    public class CreateGuestRequestInValidator : AbstractValidator<CreateGuestRequestIn>
    {
        public CreateGuestRequestInValidator()
        {
            RuleForEach(m => m.GuestRequests).ChildRules(guestRequest =>
            {
                guestRequest.RuleFor(x => x.CustomerId).GreaterThan(0);
                guestRequest.RuleFor(m => m.RequestType).IsInEnum();
                guestRequest.RuleFor(m => m.GuestId).GreaterThan(0);
                //guestRequest.RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).GreaterThan(0).When(n => n.RequestType == GuestRequestTypeEnum.EnhanceYourStay);
                guestRequest.RuleFor(m => m.CustomerGuestAppConciergeItemId).GreaterThan(0).When(n => n.RequestType == GuestRequestTypeEnum.Concierge);
                guestRequest.RuleFor(m => m.CustomerGuestAppHousekeepingItemId).GreaterThan(0).When(n => n.RequestType == GuestRequestTypeEnum.Housekeeping);
                guestRequest.RuleFor(m => m.CustomerGuestAppReceptionItemId).GreaterThan(0).When(n => n.RequestType == GuestRequestTypeEnum.Reception);
                guestRequest.RuleFor(m => m.CustomerGuestAppRoomServiceItemId).GreaterThan(0).When(n => n.RequestType == GuestRequestTypeEnum.RoomService);
            });
        }
    }
}
