using FluentValidation;

namespace HospitioApi.Core.HandleVonageRecordsLogs.Commands.CreateVonageRecordsReport
{
    public class CreateVonageRecordsReportValidator : AbstractValidator<CreateVonageRecordsReportRequest>
    {
        public CreateVonageRecordsReportValidator() 
        {
            RuleFor(m => m.In).SetValidator(new CreateVonageRecordsReportInValidator());
        }
    }
    public class CreateVonageRecordsReportInValidator : AbstractValidator<CreateVonageRecordsReportIn>
    {
    }
}
