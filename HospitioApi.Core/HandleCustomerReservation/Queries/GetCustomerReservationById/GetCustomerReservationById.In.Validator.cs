using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationById;

public class GetCustomerReservationByIdValidator : AbstractValidator<GetCustomerReservationByIdRequest>
{
    public GetCustomerReservationByIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerReservationByIdInValidator());

    }
    public class GetCustomerReservationByIdInValidator : AbstractValidator<GetCustomerReservationByIdIn>
    {
        public GetCustomerReservationByIdInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
