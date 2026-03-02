using FluentValidation;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetPaymentDetail;

public class GetPaymentDetailValidator : AbstractValidator<GetPaymentDetailRequest>
{
    public GetPaymentDetailValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetPaymentDetailInValidator());
    }
    public class GetPaymentDetailInValidator : AbstractValidator<GetPaymentDetailIn>
    {
        public GetPaymentDetailInValidator()
        {
            RuleFor(m => m.CustomerId).GreaterThan(0);
            RuleFor(m => m.GuestId).GreaterThan(0);
        }
    }
}
