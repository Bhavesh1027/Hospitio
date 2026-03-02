using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;

public class GetCustomerByGuIdValidator : AbstractValidator<GetCustomerByGuIdForHospitioRequest>
{
    public GetCustomerByGuIdValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerByGuIdInValidator());
    }

    public class GetCustomerByGuIdInValidator : AbstractValidator<GetCustomerByGuIdIn>
    {
        public GetCustomerByGuIdInValidator()
        {
            RuleFor(m => m.GuId).NotEmpty().NotNull();
        }
    }
}
