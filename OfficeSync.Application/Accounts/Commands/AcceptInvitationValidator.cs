using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class AcceptInvitationValidator : AbstractValidator<AcceptInvitationCommand>
    {
        public AcceptInvitationValidator()
        {
            RuleFor(r => r.Email)
               .NotEmpty()
               .WithMessage("Email is required.")
               .EmailAddress()
               .WithMessage("Invalid email address.");

            RuleFor(r => r.Token)
                .NotEmpty()
                .WithMessage("Token is required.");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Password is required.");

            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
