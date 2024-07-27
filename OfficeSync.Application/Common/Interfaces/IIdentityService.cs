using OfficeSync.Application.Common.Models;
using OfficeSync.Domain.Entities.Master;
using OfficeSync.Domain.Enumerations;
using OfficeSync.Domain.Interfaces;

namespace OfficeSync.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponseModel> AuthenticateAsync(string email, string password, string clientUrl, CancellationToken cancellationToken);
        Task<AuthResponseModel> VerifyMfaTokenAsync(string email, string provider, string token, CancellationToken cancellationToken);
        Task<AuthResponseModel> ResendMfaTokenAsync(string email, MfaProvider mfaProvider, string clientUrl, CancellationToken cancellationToken);
        Task<IUser> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<int> InviteCustomerAsync(int id, string email, string currentUserEmail, string clientUrl, CancellationToken cancellationToken);
        Task<bool> InviteUserAsync(int id, string currentUserEmail, string clientUrl, CancellationToken cancellationToken);
        Task<int> CreateAgentAsync(UserProfile userProfile, string email, string clientUrl, CancellationToken cancellationToken);
        Task<bool> AcceptInvitationAsync(string email, string password, string token, string clientUrl, CancellationToken cancellationToken);
        Task<string> GenerateEmailConfirmationTokenAsync(IUser user);
        Task<bool> RequestChangePasswordAsync(string email, string clientUrl, CancellationToken cancellationToken);
        Task<bool> ChangePasswordAsync(string email, string token, string password, string clientUrl, CancellationToken cancellationToken);
        Task<bool> RequestVerificationCodeAsync(string email, string phoneNumber, string clientUrl, CancellationToken cancellationToken);
        Task<bool> VerifyPhoneNumberAsync(string email, string token, string phoneNumber, string clientUrl, CancellationToken cancellationToken);
        Task<bool> RemovePhoneNumberAsync(string phoneNumber, string email, string clientUrl, CancellationToken cancellationToken);
        Task<bool> EnableMfaAsync(string email, MfaProvider defaultProvider, string clientUrl, CancellationToken cancellationToken);
        Task<bool> DisableMfaAsync(string email, string clientUrl, CancellationToken cancellationToken);
        IQueryable<IUser> QueryCustomers();
        Task<List<UserModel>> ListAgentsAsync(CancellationToken cancellationToken);
        Task<MfaProvider[]> ListUserMfaProvidersAsync(string email, CancellationToken cancellationToken);
    }
}
