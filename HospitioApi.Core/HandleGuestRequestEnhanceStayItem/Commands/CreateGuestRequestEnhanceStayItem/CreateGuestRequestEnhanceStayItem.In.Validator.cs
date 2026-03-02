using FluentValidation;

namespace HospitioApi.Core.HandleGuestRequestEnhanceStayItem.Commands.CreateGuestRequestEnhanceStayItem;

public class CreateGuestRequestEnhanceStayItemValidator : AbstractValidator<CreateGuestRequestEnhanceStayItemRequest>
{
    public CreateGuestRequestEnhanceStayItemValidator()
    {
        RuleFor(m => m.In).SetValidator(new CreateGuestRequestInValidator());
    }
    public class CreateGuestRequestInValidator : AbstractValidator<CreateGuestRequestEnhanceStayItemIn>
    {
        public CreateGuestRequestInValidator()
        {
            RuleFor(m => m.CustomerId).NotNull().GreaterThan(0);
            RuleFor(m => m.GuestId).NotNull().GreaterThan(0);
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotNull().GreaterThan(0);
            RuleForEach(m => m.enhanceStayItemExtraIns).ChildRules(guestRequest =>
            {
                guestRequest.RuleFor(x => x.CustomerGuestAppEnhanceYourStayCategoryItemsExtraId).NotNull().GreaterThan(0);
            });
        }
    }
}
