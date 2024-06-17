using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class DisableMfaValidator : AbstractValidator<DisableMfaCommand>
    {
        public DisableMfaValidator()
        {
            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
