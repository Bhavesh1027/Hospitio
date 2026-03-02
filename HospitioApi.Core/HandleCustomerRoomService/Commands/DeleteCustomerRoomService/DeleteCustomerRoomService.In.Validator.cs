using FluentValidation;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;

public class DeleteCustomerRoomServiceValidator : AbstractValidator<DeleteCustomerRoomServiceRequest>
{
    public DeleteCustomerRoomServiceValidator()
    {
        RuleFor(m => m.In).SetValidator(new DeleteCustomerRoomServiceInValidator());
    }
    public class DeleteCustomerRoomServiceInValidator : AbstractValidator<DeleteCustomerRoomServiceIn>
    {
        public DeleteCustomerRoomServiceInValidator()
        {
            RuleFor(m => m.Id).NotEmpty().GreaterThan(0);
        }
    }
}
