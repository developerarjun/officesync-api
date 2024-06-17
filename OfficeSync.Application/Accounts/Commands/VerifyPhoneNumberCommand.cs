using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class VerifyPhoneNumberHandler : IRequestHandler<VerifyPhoneNumberCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public VerifyPhoneNumberHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(VerifyPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.VerifyPhoneNumberAsync(request.CurrentUserEmail, request.Token, request.PhoneNumber, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class VerifyPhoneNumberCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }
}
