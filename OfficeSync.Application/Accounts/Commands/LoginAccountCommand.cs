using MediatR;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Commands
{
    public class LoginAccountHandler : IRequestHandler<LoginAccountCommand, LoginAccountResponse>
    {
        private readonly IIdentityService _identityService;
        public LoginAccountHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginAccountResponse> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _identityService.AuthenticateAsync(request.Email, request.Password, request.ClientUrl, cancellationToken);

            var response = new LoginAccountResponse
            {
                FullName = authResult.FullName,
                TokenType = authResult.TokenType,
                Token = authResult.Token
            };

            return response;
        }
    }

    public class LoginAccountCommand : IRequest<LoginAccountResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string ClientUrl { get; set; }
    }

    public class LoginAccountResponse
    {
        public string FullName { get; set; }
        public AuthTokenType TokenType { get; set; }
        public string Token { get; set; }
    }
}
