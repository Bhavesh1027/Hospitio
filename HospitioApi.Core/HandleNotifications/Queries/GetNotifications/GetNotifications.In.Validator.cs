using FluentValidation;

namespace HospitioApi.Core.HandleNotifications.Queries.GetNotifications;

public class GetNotificationsValidator : AbstractValidator<GetNotificationsRequest>
{
    public GetNotificationsValidator()
    {
        RuleFor(m => m.In).SetValidator(new GetNotificationsInValidator());
    }
    public class GetNotificationsInValidator : AbstractValidator<GetNotificationsIn>
    {
        public GetNotificationsInValidator()
        {
            RuleFor(m => m.PageNo).NotEmpty().GreaterThan(0);
            RuleFor(m => m.PageSize).NotEmpty().GreaterThan(0);
        }
    }
}
