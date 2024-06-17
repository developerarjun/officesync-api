using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class VerifyPhoneNumberValidator : AbstractValidator<VerifyPhoneNumberCommand>
    {
        public VerifyPhoneNumberValidator()
        {
            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");

            RuleFor(r => r.Token)
                .NotEmpty()
                .WithMessage("Token is required.");

            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
