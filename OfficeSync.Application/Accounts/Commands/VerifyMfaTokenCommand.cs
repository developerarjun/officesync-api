using MediatR;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class VerifyMfaTokenHandler : IRequestHandler<VerifyMfaTokenCommand, VerifyMfaTokenResponse>
    {
        private readonly IIdentityService _identityService;
        public VerifyMfaTokenHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<VerifyMfaTokenResponse> Handle(VerifyMfaTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.VerifyMfaTokenAsync(request.CurrentUserEmail, request.Provider, request.Token, cancellationToken);

            var response = new VerifyMfaTokenResponse
            {
                FullName = result.FullName,
                TokenType = result.TokenType,
                Token = result.Token
            };
            return response;
        }
    }

    public class VerifyMfaTokenCommand : IRequest<VerifyMfaTokenResponse>
    {
        public string Token { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
        [JsonIgnore]
        public string Provider { get; set; }
    }

    public class VerifyMfaTokenResponse
    {
        public string FullName { get; set; }
        public AuthTokenType TokenType { get; set; }
        public string Token { get; set; }
    }
}
