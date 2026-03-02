using FluentValidation;

namespace HospitioApi.Core.HandlePaymentDetails.Queries.GetAdminPaymentDetail;

public class GetAdminPaymentDetailValidator : AbstractValidator<GetAdminPaymentDetailRequest>
{
    public GetAdminPaymentDetailValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetAdminPaymentDetailInValidator());
    }

    public class GetAdminPaymentDetailInValidator : AbstractValidator<GetAdminPaymentDetailIn>
    {
        public GetAdminPaymentDetailInValidator()
        {
            RuleFor(m => m.GuestId).GreaterThan(0);
        }
    }
}