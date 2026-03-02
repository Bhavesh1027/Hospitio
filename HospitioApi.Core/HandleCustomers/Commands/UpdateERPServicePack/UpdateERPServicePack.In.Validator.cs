using FluentValidation;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack
{
    public class UpdateERPServicePackValidator : AbstractValidator<UpdateERPServicePackRequest>
    {
        public UpdateERPServicePackValidator()
        {
            RuleFor(m => m.In).SetValidator(new UpdateERPServiceInValidation());
        }
    }
    public class UpdateERPServiceInValidation : AbstractValidator<UpdateERPServicePackIn>
    {
        public UpdateERPServiceInValidation()
        {
            RuleFor(m => m.PylonUniqueCustomerId).NotNull().NotEmpty();
            RuleFor(m => m.ServicePack).NotNull().NotEmpty();
            RuleFor(m => m.ExpirationInDay).NotNull();
            RuleFor(m => m.PurchaseType).NotNull().NotEmpty();
        }
    }
}
