using FluentValidation;

namespace HospitioApi.Core.HandleCustomerUsers.Commands.UpdateMuteStatus;

public class UpdateMuteStatusValidator : AbstractValidator<UpdateCustomerMuteStatusRequest>
{
	public UpdateMuteStatusValidator()
	{
		RuleFor(m => m.In).SetValidator(new UpdateCustomerUserMuteStatusInValidator());
	}

	public class UpdateCustomerUserMuteStatusInValidator : AbstractValidator<UpdateMuteStatusIn>
	{
		public UpdateCustomerUserMuteStatusInValidator()
		{
		}
	}
}
