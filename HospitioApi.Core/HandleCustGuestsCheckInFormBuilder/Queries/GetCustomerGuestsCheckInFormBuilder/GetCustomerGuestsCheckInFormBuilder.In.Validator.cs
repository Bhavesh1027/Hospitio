using FluentValidation;

namespace HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
public class GetCustomerGuestsCheckInFormBuilderValidator : AbstractValidator<GetCustomerGuestsCheckInFormBuilderRequest>
{
    public GetCustomerGuestsCheckInFormBuilderValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetCustomerGuestsCheckInFormBuilderInValidator());
    }
    public class GetCustomerGuestsCheckInFormBuilderInValidator : AbstractValidator<GetCustomerGuestsCheckInFormBuilderIn>
    {
        public GetCustomerGuestsCheckInFormBuilderInValidator()
        {
        }
    }
}