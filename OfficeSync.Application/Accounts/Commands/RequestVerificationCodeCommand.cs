using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class RequestVerificationCodeHandler : IRequestHandler<RequestVerificationCodeCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public RequestVerificationCodeHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(RequestVerificationCodeCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RequestVerificationCodeAsync(request.CurrentUserEmail, request.PhoneNumber, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class RequestVerificationCodeCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }
}
