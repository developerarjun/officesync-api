using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OfficeSync.Application.Common.Exceptions;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Application.Common.Models;
using OfficeSync.Domain.Entities;
using OfficeSync.Domain.Enumerations;
using OfficeSync.Domain.Interfaces;
using OfficeSync.Infrastructure.Common.Options;
using OfficeSync.Infrastructure.Persistence.Identity;
using OfficeSync.Infrastructure.Persistence.Initializers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace OfficeSync.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtOptions _jwtOptions;
        public IdentityService(UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               IOptions<JwtOptions> options)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtOptions = options.Value;
        }

        public async Task<AuthResponseModel> AuthenticateAsync(string email, string password, string clientUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(i => i.ProfileRef)
                                               .Include(i => i.Roles)
                                               .Where(w => w.NormalizedEmail == email.ToUpper())
                                               .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new BadRequestException("Invalid email or password.");

            var result = await _signInManager.PasswordSignInAsync(user, password, true, true);

            if (result.IsNotAllowed)
                throw new BadRequestException("Account confirmation process not completed.");
            else if (result.IsLockedOut)
            {
                var remainingSeconds = user.LockoutEnd.Value.Subtract(DateTimeOffset.UtcNow).TotalSeconds;
                throw new AccountLockedException($"{remainingSeconds}");
            }
            else if (result.RequiresTwoFactor)
                return await GetMfaTokenAsync(user, clientUrl, cancellationToken);
            else if (!result.Succeeded)
                throw new BadRequestException("Invalid email or password.");

            var token = GenerateToken(user);
            var response = new AuthResponseModel
            {
                FullName = $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}",
                TokenType = AuthTokenType.JWT,
                Token = token
            };
            return response;
        }

        private async Task<AuthResponseModel> GetMfaTokenAsync(User user, string clientUrl, CancellationToken cancellationToken)
        {
            var mfaToken = await _userManager.GenerateTwoFactorTokenAsync(user, $"{user.DefaultMfaProvider}");

            user.LastUpdatedAt = DateTimeOffset.UtcNow;
            user.LastUpdatedBy = user.Email;
            user.EventActivity = user.DefaultMfaProvider == MfaProvider.Phone ? UserEventActivity.MfaTokenToPhone : UserEventActivity.MfaTokenToEmail;
            user.Token = mfaToken;
            user.Link = clientUrl;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }

            var response = new AuthResponseModel
            {
                FullName = $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}",
                TokenType = AuthTokenType.MFA,
                Token = GenerateToken(user, true, $"{user.DefaultMfaProvider}")
            };

            return response;
        }

        public async Task<AuthResponseModel> ResendMfaTokenAsync(string email, MfaProvider mfaProvider, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Include(i => i.Roles)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper())
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            if (mfaProvider == MfaProvider.Phone && !dbUser.PhoneNumberConfirmed ||
                mfaProvider == MfaProvider.Email && !dbUser.EmailConfirmed)
                throw new BadRequestException("Invalid mfa profider.");

            var mfaToken = await _userManager.GenerateTwoFactorTokenAsync(dbUser, $"{mfaProvider}");

            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = email;
            dbUser.EventActivity = mfaProvider == MfaProvider.Phone ? UserEventActivity.MfaTokenToPhone : UserEventActivity.MfaTokenToEmail;
            dbUser.Token = mfaToken;
            dbUser.Link = clientUrl;

            var result = await _userManager.UpdateAsync(dbUser);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }

            var response = new AuthResponseModel
            {
                FullName = $"{dbUser.ProfileRef.FirstName} {dbUser.ProfileRef.LastName}",
                TokenType = AuthTokenType.MFA,
                Token = GenerateToken(dbUser, true, $"{mfaProvider}")
            };
            return response;
        }

        public async Task<AuthResponseModel> VerifyMfaTokenAsync(string email,
                                                                 string provider,
                                                                 string token,
                                                                 CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(i => i.ProfileRef)
                                               .Include(i => i.Roles)
                                               .Where(w => w.NormalizedEmail == email.ToUpper())
                                               .AsNoTracking()
                                               .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                throw new BadRequestException("Invalid token.");

            var result = await _userManager.VerifyTwoFactorTokenAsync(user, provider, token);
            if (result)
            {
                await _signInManager.SignInAsync(user, false);

                var response = new AuthResponseModel
                {
                    FullName = $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}",
                    TokenType = AuthTokenType.JWT,
                    Token = GenerateToken(user)
                };

                return response;
            }

            throw new BadRequestException("Invalid token.");
        }

        public async Task<IUser> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper())
                                                 .FirstOrDefaultAsync(cancellationToken);

            return dbUser;
        }

        public async Task<int> InviteCustomerAsync(int id, string email, string currentUserEmail, string clientUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
                throw new BadRequestException("Email address already registered.");

            user = new User();
            user.Id = id;
            user.UserName = email;
            user.Email = email;
            user.LastUpdatedAt = DateTimeOffset.UtcNow;
            user.LastUpdatedBy = currentUserEmail;
            user.RoleName = RoleInitializer.CUSTOMER_NAME;
            user.Roles = new List<UserRole> { new UserRole { RoleId = RoleInitializer.CUSTOMER } };

            user.EventActivity = UserEventActivity.Invite;
            user.Link = clientUrl;

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return user.Id;
        }

        public async Task<bool> InviteUserAsync(int id, string currentUserEmail, string clientUrl, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.Include(i => i.Roles)
                                               .ThenInclude(i => i.RoleRef)
                                               .Where(w => w.Id == id)
                                               .FirstOrDefaultAsync(cancellationToken);
            if (user != null)
                throw new NotFoundException();

            user.EventActivity = UserEventActivity.Invite;
            user.Link = clientUrl;
            user.RoleName = user.Roles.FirstOrDefault()?.RoleRef?.Name;
            user.LastUpdatedAt = DateTimeOffset.UtcNow;
            user.LastUpdatedBy = currentUserEmail;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<int> CreateAgentAsync(UserProfile userProfile, string email, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = new User
            {
                UserName = email,
                Email = email,
                LastUpdatedAt = DateTimeOffset.UtcNow,
                LastUpdatedBy = userProfile.LastUpdatedBy,
                Roles = new List<UserRole> { new UserRole { RoleId = RoleInitializer.AGENT } },
                ProfileRef = userProfile,
                EventActivity = UserEventActivity.Invite,
                Link = clientUrl,
                RoleName = RoleInitializer.AGENT_NAME,
                Token = userProfile.LastUpdatedBy
            };

            var result = await _userManager.CreateAsync(dbUser);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return dbUser.Id;
        }

        public async Task<bool> AcceptInvitationAsync(string email, string password, string token, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper())
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new BadRequestException("Invalid token.");

            var codeEncodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeEncoded = Encoding.UTF8.GetString(codeEncodedBytes);

            var confirmResult = await _userManager.ConfirmEmailAsync(dbUser, codeEncoded);
            if (!confirmResult.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(confirmResult.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }

            dbUser.EventActivity = UserEventActivity.AcceptInvitation;
            dbUser.Link = clientUrl;
            dbUser.EmailConfirmed = true;
            dbUser.IsActive = true;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = email;
            // Always add password after the email has been confirmed.
            var passwordResult = await _userManager.AddPasswordAsync(dbUser, password);
            if (!passwordResult.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(passwordResult.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }

            return passwordResult.Succeeded;
        }

        public async Task<MfaProvider[]> ListUserMfaProvidersAsync(string email, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();

            var proviers = new List<MfaProvider>();
            if (dbUser.EmailConfirmed)
                proviers.Add(MfaProvider.Email);
            if (dbUser.PhoneNumberConfirmed)
                proviers.Add(MfaProvider.Phone);

            return proviers.ToArray();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(IUser user)
        {
            var dbUser = (User)user;
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(dbUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

            return codeEncoded;
        }

        public async Task<bool> RequestChangePasswordAsync(string email, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper() &&
                                                             w.IsActive == true &&
                                                             w.EmailConfirmed == true)
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);
            byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
            var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

            dbUser.EventActivity = UserEventActivity.RequestChangePassword;
            dbUser.Link = $"{clientUrl}/accounts/reset-password?firstname={dbUser.ProfileRef.FirstName}&lastname={dbUser.ProfileRef.LastName}&email={dbUser.Email}&token={codeEncoded}";
            dbUser.Token = codeEncoded;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.UpdateAsync(dbUser);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> ChangePasswordAsync(string email, string token, string password, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Where(w => w.NormalizedEmail == email.ToUpper() &&
                                                             w.IsActive == true &&
                                                             w.EmailConfirmed == true)
                                                 .FirstOrDefaultAsync(cancellationToken);
            if (dbUser == null)
                throw new NotFoundException();

            var codeEncodedBytes = WebEncoders.Base64UrlDecode(token);
            var codeEncoded = Encoding.UTF8.GetString(codeEncodedBytes);

            dbUser.EventActivity = UserEventActivity.ChangePassword;
            dbUser.Link = clientUrl;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.ResetPasswordAsync(dbUser, codeEncoded, password);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> RequestVerificationCodeAsync(string email, string phoneNumber, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();

            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(dbUser, phoneNumber);

            dbUser.EventActivity = UserEventActivity.ChangePhoneNumber;
            dbUser.Token = token;
            dbUser.Link = clientUrl;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.UpdateAsync(dbUser);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> VerifyPhoneNumberAsync(string email, string token, string phoneNumber, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();

            dbUser.EventActivity = UserEventActivity.VerifyPhoneNumber;
            dbUser.Link = clientUrl;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.ChangePhoneNumberAsync(dbUser, phoneNumber, token);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> RemovePhoneNumberAsync(string phoneNumber, string email, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();
            if (dbUser.PhoneNumber != phoneNumber)
                throw new NotFoundException();

            dbUser.EventActivity = UserEventActivity.RemovePhoneNumber;
            dbUser.Link = clientUrl;
            dbUser.PhoneNumberConfirmed = false;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.SetPhoneNumberAsync(dbUser, null);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> EnableMfaAsync(string email, MfaProvider defaultProvider, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();

            dbUser.EventActivity = UserEventActivity.Enable2FA;
            dbUser.DefaultMfaProvider = defaultProvider;
            dbUser.Link = clientUrl;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.SetTwoFactorEnabledAsync(dbUser, true);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        public async Task<bool> DisableMfaAsync(string email, string clientUrl, CancellationToken cancellationToken)
        {
            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser == null)
                throw new NotFoundException();

            dbUser.EventActivity = UserEventActivity.Disable2FA;
            dbUser.Link = clientUrl;
            dbUser.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbUser.LastUpdatedBy = dbUser.Email;

            var result = await _userManager.SetTwoFactorEnabledAsync(dbUser, false);
            if (!result.Succeeded)
            {
                var errorMessage = JsonSerializer.Serialize(result.Errors.Select(s => s.Description).ToArray());
                throw new BadRequestException(errorMessage);
            }
            return result.Succeeded;
        }

        private string GenerateToken(User user, bool isMfa = false, string mfaProvider = "")
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = Encoding.UTF8.GetBytes(_jwtOptions.Key);
            var securityKey = new SymmetricSecurityKey(secretKey);
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Name, $"{user.ProfileRef.FirstName} {user.ProfileRef.LastName}"),
                new Claim(ClaimTypes.Email, $"{user.Email}")
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            claimsIdentity.AddClaims(user.Roles.Select(s => new Claim(ClaimTypes.Role, $"{s.RoleId}")));

            if (isMfa)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.AuthenticationMethod, "mfa"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.AuthorizationDecision, mfaProvider));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireInMinutes),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512),
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public IQueryable<IUser> QueryCustomers()
        {
            var customers = _userManager.Users.Include(i => i.Roles)
                                              .Where(w => w.Roles.Any(a => a.RoleId == RoleInitializer.CUSTOMER))
                                              .Select(s => new User
                                              {
                                                  Id = s.Id,
                                                  Email = s.Email,
                                                  EmailConfirmed = s.EmailConfirmed
                                              });

            return customers;
        }

        public async Task<List<UserModel>> ListAgentsAsync(CancellationToken cancellationToken)
        {
            var agents = await _userManager.Users.Include(i => i.ProfileRef)
                                                 .Include(i => i.Roles)
                                                 .ThenInclude(i => i.RoleRef)
                                                 .Where(w => w.Roles.Any(a => a.RoleId == RoleInitializer.AGENT))
                                                 .Select(s => new UserModel
                                                 {
                                                     Id = s.Id,
                                                     Suffix = s.ProfileRef.Suffix,
                                                     FirstName = s.ProfileRef.FirstName,
                                                     LastName = s.ProfileRef.LastName,
                                                     MiddleName = s.ProfileRef.MiddleName,
                                                     Email = s.Email,
                                                     IsEmailVerified = s.EmailConfirmed,
                                                     PhoneNumber = s.PhoneNumber,
                                                     IsPhoneNumberVerified = s.PhoneNumberConfirmed,
                                                     IsMfaEnabled = s.TwoFactorEnabled,
                                                     Role = s.Roles.Any() ? s.Roles.FirstOrDefault().RoleRef.Name : ""
                                                 })
                                                 .ToListAsync(cancellationToken);

            return agents;
        }
    }
}
