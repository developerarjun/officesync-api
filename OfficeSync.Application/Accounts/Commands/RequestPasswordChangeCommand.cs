using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class RequestPasswordChoangeHandler : IRequestHandler<RequestPasswordChangeCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public RequestPasswordChoangeHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(RequestPasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RequestChangePasswordAsync(request.Email, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class RequestPasswordChangeCommand : IRequest<bool>
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
    }
}
