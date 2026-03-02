using FluentValidation;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;

public class UpdateCustomerRoomServiceValidator : AbstractValidator<UpdateCustomerRoomServiceRequest>
{
    public UpdateCustomerRoomServiceValidator()
    {
         RuleFor(m => m.In).SetValidator(new UpdateCustomerRoomServiceInValidator());
    }

    public class UpdateCustomerRoomServiceInValidator : AbstractValidator<UpdateCustomerRoomServiceIn>
    {
        public UpdateCustomerRoomServiceInValidator()
        {
            //RuleForEach(m => m.UpdateCustomerRoomServiceCategoryIn).ChildRules(q =>
            //{
            //    q.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
            //    q.RuleFor(m => m.CategoryName).NotEmpty();
            //    q.RuleForEach(m => m.CustomerRoomServiceItems).ChildRules(c =>
            //    {
            //        c.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty().GreaterThan(0);
            //        c.RuleFor(m => m.CategoryName).MaximumLength(25);
            //        c.RuleFor(m => m.Name).MaximumLength(200);
            //        c.RuleFor(m => m.Currency).MaximumLength(3);
            //    });
            //});
        }
    }
}
