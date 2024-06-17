using MediatR;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class ResendMfaTokenHandler : IRequestHandler<ResendMfaTokenCommand, ResendMfaTokenResponse>
    {
        private readonly IIdentityService _identityService;
        public ResendMfaTokenHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ResendMfaTokenResponse> Handle(ResendMfaTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ResendMfaTokenAsync(request.CurrentUserEmail, request.MfaProvider, request.ClientUrl, cancellationToken);

            var response = new ResendMfaTokenResponse
            {
                FullName = result.FullName,
                TokenType = result.TokenType,
                Token = result.Token
            };
            return response;
        }
    }

    public class ResendMfaTokenCommand : IRequest<ResendMfaTokenResponse>
    {
        public MfaProvider MfaProvider { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }

    public class ResendMfaTokenResponse
    {
        public string FullName { get; set; }
        public AuthTokenType TokenType { get; set; }
        public string Token { get; set; }
    }
}
