using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile
{
    public class UpdateUserProfileValidator : AbstractValidator<UpdateUserProfileRequest>
    {
        public UpdateUserProfileValidator()
        {
            RuleFor(m => m.In).SetValidator(new UpdateUserProfileInValidator());
        }
        public class UpdateUserProfileInValidator : AbstractValidator<UpdateUserProfileIn>
        {
            public UpdateUserProfileInValidator()
            {
                RuleFor(m => m.UserId).GreaterThan(0);
            }
        }
    }
}
