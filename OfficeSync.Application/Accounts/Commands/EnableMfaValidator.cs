using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class EnableMfaValidator : AbstractValidator<EnableMfaCommand>
    {
        public EnableMfaValidator()
        {
            RuleFor(r => r.DefaultMfaProvider)
                .IsInEnum()
                .WithMessage("Invalid mfa provider.");

            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
