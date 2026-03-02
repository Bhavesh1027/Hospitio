using FluentValidation;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Queries.GetCustomerGuestAppEnhanceYourStayItemsExtraByStayId;

public class GetCustomerGuestAppEnhannceYourStayItemExtraValidator : AbstractValidator<GetCustomerGuestAppEnhannceYourStayItemExtraRequest>
{
    public GetCustomerGuestAppEnhannceYourStayItemExtraValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestAppEnhannceYourStayItemExtraInValidator());

    }
    public class GetGuestAppEnhannceYourStayItemExtraInValidator : AbstractValidator<GetCustomerGuestAppEnhannceYourStayItemExtraIn>
    {
        public GetGuestAppEnhannceYourStayItemExtraInValidator()
        {
            RuleFor(m => m.CustomerGuestAppEnhanceYourStayItemId).NotEmpty().GreaterThan(0);
        }
    }
}
