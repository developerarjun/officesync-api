using MediatR;
using OfficeSync.Application.Common.Interfaces;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class RemovePhoneNumberHandler : IRequestHandler<RemovePhoneNumberCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public RemovePhoneNumberHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(RemovePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RemovePhoneNumberAsync(request.PhoneNumber, request.CurrentUserEmail, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class RemovePhoneNumberCommand : IRequest<bool>
    {
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }
}
