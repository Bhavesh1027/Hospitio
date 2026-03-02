using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerLatitudeLongitude;

public class GetCustomerLatitudeLongitudeValidator : AbstractValidator<GetCustomerLatitudeLongitudeRequest>
{
    public GetCustomerLatitudeLongitudeValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerLatitudeLongitudeInValidator());
    }
    public class GetCustomerLatitudeLongitudeInValidator : AbstractValidator<GetCustomerLatitudeLongitudeIn>
    {
        public GetCustomerLatitudeLongitudeInValidator()
        {
            RuleFor(m => m.BuilderId).NotEmpty();
            RuleFor(m => m.CustomerId).NotEmpty();
        }
    }
}
