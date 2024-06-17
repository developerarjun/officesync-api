using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class AcceptInvitationHandler : IRequestHandler<AcceptInvitationCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public AcceptInvitationHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.AcceptInvitationAsync(request.Email, request.Password, request.Token, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class AcceptInvitationCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
    }
}
