using FluentValidation;

namespace HospitioApi.Core.HandleAccount.Commands.EncryptPasswords;
public class EncryptPasswordsHandlerRequestValidator : AbstractValidator<EncryptPasswordsHandlerRequest>
{
    public EncryptPasswordsHandlerRequestValidator()
    {
        RuleFor(m => m.In).SetValidator(new EncryptPasswordsInValidator());
    }

    public class EncryptPasswordsInValidator : AbstractValidator<EncryptPasswordsIn>
    {
        public EncryptPasswordsInValidator()
        {
            
        }
    }
}