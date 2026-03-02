using FluentValidation;

namespace HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;

public class GetDisplayOrderValidator : AbstractValidator<GetDisplayOrderRequest>
{
    public GetDisplayOrderValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetDisplayOrderInValidator());
    }

    public class GetDisplayOrderInValidator : AbstractValidator<GetDisplayOrderIn>
    {
        public GetDisplayOrderInValidator()
        {
            RuleFor(m => m.ReferenceId).NotEmpty().GreaterThan(0);
        }
    }
}
