using FluentValidation;

namespace HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData
{
    public class ReplicateDataValidator :AbstractValidator<ReplicateDataRequest>
    {
        public ReplicateDataValidator()
        {
            RuleFor(m => m.In).SetValidator(new ReplicatedDataValidator());
        }
        public class ReplicatedDataValidator : AbstractValidator<ReplicateDataIn>
        {
            public ReplicatedDataValidator()
            {
                RuleFor(m => m.AppBuilderId).NotEmpty().GreaterThan(0);

                RuleFor(m => m.NewBuilderId).NotEmpty().GreaterThan(0);
               
            }
        }
    }
}
