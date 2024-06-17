using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class VerifyMfaTokenValidator : AbstractValidator<VerifyMfaTokenCommand>
    {
        public VerifyMfaTokenValidator()
        {
            RuleFor(r => r.Token)
                .NotEmpty()
                .WithMessage("Token is required.");
        }
    }
}
