using FluentValidation;

namespace HospitioApi.Core.HandleTransactions.Commands.CaptureAdminTransaction;

public class CaptureAdminTransactionValidator : AbstractValidator<CaptureAdminTransactionRequest>
{
    public CaptureAdminTransactionValidator()
    {
        RuleFor(m => m.In).SetValidator(new CaptureAdminTransactionInValidator());
    }
    public class CaptureAdminTransactionInValidator : AbstractValidator<CaptureAdminTransactionIn>
    {
        public CaptureAdminTransactionInValidator()
        {
            RuleFor(m => m.Transaction_Id).NotNull().NotEmpty();
            RuleFor(m => m.Amount).NotNull().GreaterThan(0);
        }
    }
}