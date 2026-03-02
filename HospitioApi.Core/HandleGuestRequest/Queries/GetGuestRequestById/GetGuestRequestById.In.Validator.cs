using FluentValidation;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequestById;
                                                                  
public class GetGuestRequestByIdByIdValidator : AbstractValidator<GetGuestRequestByIdRequest>
{
    public GetGuestRequestByIdByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetGuestRequestByIdInValidator());
    }
    public class GetGuestRequestByIdInValidator : AbstractValidator<GetGuestRequestByIdIn>
    {
        public GetGuestRequestByIdInValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }
}
