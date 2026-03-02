using FluentValidation;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservations;

public class GetCustomerReservationsValidator : AbstractValidator<GetCustomerReservationsRequest>
{
    public GetCustomerReservationsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerReservationsInValidator());

    }
    public class GetCustomerReservationsInValidator : AbstractValidator<GetCustomerReservationsIn>
    {
        public GetCustomerReservationsInValidator()
        {
            RuleFor(m => m.CustomerId).NotEmpty().NotNull().GreaterThan(0);
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
