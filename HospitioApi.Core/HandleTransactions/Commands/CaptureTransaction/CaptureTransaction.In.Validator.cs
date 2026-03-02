using FluentValidation;

namespace HospitioApi.Core.HandleTransactions.Commands.CaptureTransaction;

public class CaptureTransactionValidator : AbstractValidator<CaptureTransactionRequest>
{
    public CaptureTransactionValidator()
    {
        RuleFor(m => m.In).SetValidator(new CaptureTransactionInValidator());
    }
    public class CaptureTransactionInValidator : AbstractValidator<CaptureTransactionIn>
    {
        public CaptureTransactionInValidator()
        {
            RuleFor(m => m.CustomerId).NotNull().GreaterThan(0);
            RuleFor(m => m.Transaction_Id).NotNull().NotEmpty();
            RuleFor(m => m.Amount).NotNull().GreaterThan(0);
        }
    }
}
