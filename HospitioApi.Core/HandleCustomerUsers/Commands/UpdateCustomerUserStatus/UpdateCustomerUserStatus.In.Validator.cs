using FluentValidation;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.UpdateCustomerUserStatus;

public class UpdateCustomerUserStatusValidator : AbstractValidator<UpdateCustomerUserStatusRequest>
{
	public UpdateCustomerUserStatusValidator()
	{
		RuleFor(m => m.In).SetValidator(new UpdateCustomerUserStatusInValidator());
	}

	public class UpdateCustomerUserStatusInValidator : AbstractValidator<UpdateCustomerUserStatusIn>
	{
		public UpdateCustomerUserStatusInValidator()
		{
			RuleFor(m => m.CustomerUserId).GreaterThan(0);
		}
	}
}