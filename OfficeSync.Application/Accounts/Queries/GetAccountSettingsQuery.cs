using MediatR;
using OfficeSync.Application.Common.Exceptions;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace OfficeSync.Application.Accounts.Queries
{
    public class GetAccountSettingsHandler : IRequestHandler<GetAccountSettingsQuery, GetAccountSettingsResponse>
    {
        private readonly IIdentityService _identityService;
        public GetAccountSettingsHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<GetAccountSettingsResponse> Handle(GetAccountSettingsQuery request, CancellationToken cancellationToken)
        {
            var dbUser = await _identityService.GetByEmailAsync(request.CurrentUserEmail, cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            var response = new GetAccountSettingsResponse
            {
                FirstName = dbUser.ProfileRef.FirstName,
                LastName = dbUser.ProfileRef.LastName,
                MiddleName = dbUser.ProfileRef.MiddleName,
                Email = dbUser.Email,
                IsEmailConfirmed = dbUser.EmailConfirmed,
                PhoneNumber = dbUser.PhoneNumber,
                IsPhoneNumberConfirmed = dbUser.PhoneNumberConfirmed,
                IsMfaEnabled = dbUser.TwoFactorEnabled,
                DefaultMfaProvider = dbUser.DefaultMfaProvider
            };

            return response;
        }
    }

    public class GetAccountSettingsQuery : IRequest<GetAccountSettingsResponse>
    {
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }

    public class GetAccountSettingsResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsPhoneNumberConfirmed { get; set; }
        public bool IsMfaEnabled { get; set; }
        public MfaProvider DefaultMfaProvider { get; set; }
    }
}
