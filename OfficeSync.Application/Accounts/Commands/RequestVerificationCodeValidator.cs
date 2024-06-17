using FluentValidation;

namespace OfficeSync.Application.Accounts.Commands
{
    public class RequestVerificationCodeValidator : AbstractValidator<RequestVerificationCodeCommand>
    {
        public RequestVerificationCodeValidator()
        {
            RuleFor(r => r.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required.");

            RuleFor(r => r.ClientUrl)
                .NotEmpty()
                .WithMessage("Client url is required.");
        }
    }
}
