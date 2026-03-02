using FluentValidation;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;

public class CreateCustomerRoomServiceValidator : AbstractValidator<CreateCustomerRoomServiceRequest>
{
    public CreateCustomerRoomServiceValidator()
    {
        // RuleFor(m => m.In).SetValidator(new CreateCustomerRoomServiceInValidator());
    }
    public class CreateCustomerRoomServiceInValidator : AbstractValidator<CreateCustomerRoomServiceCategoryIn>
    {
        //public CreateCustomerRoomServiceInValidator()
        //{
        //    RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        //    RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty();
        //    RuleFor(m => m.CategoryName).NotEmpty();
        //    RuleForEach(m => m.CustomerRoomServiceItems).ChildRules(c =>
        //    {
        //        c.RuleFor(m => m.CustomerId).NotEmpty().GreaterThan(0);
        //        c.RuleFor(m => m.CustomerGuestAppBuilderId).NotEmpty().GreaterThan(0);
        //        c.RuleFor(m => m.CategoryName).MaximumLength(25);
        //        c.RuleFor(m => m.Name).MaximumLength(200);
        //        c.RuleFor(m => m.Currency).MaximumLength(3);
        //    });
        //}
    }
}
