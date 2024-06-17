using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class DisableMfaHandler : IRequestHandler<DisableMfaCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public DisableMfaHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(DisableMfaCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.DisableMfaAsync(request.CurrentUserEmail, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class DisableMfaCommand : IRequest<bool>
    {
        [JsonIgnore]
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }
}
