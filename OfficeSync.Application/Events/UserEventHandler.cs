using MediatR;
using OfficeSync.Application.Common.Events;
using OfficeSync.Application.Common.Helpers;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Application.Common.Models;
using OfficeSync.Domain.Enumerations;
using OfficeSync.Domain.Interfaces;

namespace OfficeSync.Application.Events
{
    public class UserEventHandler : INotificationHandler<CreatedEvent>, INotificationHandler<UpdatedEvent>
    {
        private readonly IIdentityService _identityService;
        private readonly ITwilioService _twilioService;
        private readonly IEmailService _emailService;
        public UserEventHandler(IIdentityService identityService,
                                ITwilioService twilioService,
                                IEmailService emailService)
        {
            _identityService = identityService;
            _twilioService = twilioService;
            _emailService = emailService;
        }

        public async Task Handle(CreatedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.GetEntity<IUser>();
            if (user != null)
            {
                if (user.EventActivity == UserEventActivity.Invite)
                {
                    await NotifyInvitationAsync(user, cancellationToken);
                }
            }
        }

        public async Task Handle(UpdatedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.GetEntity<IUser>();
            if (user != null)
            {
                switch (user.EventActivity)
                {
                    case UserEventActivity.Invite:
                        await NotifyInvitationAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.AcceptInvitation:
                        await NotifyAcceptInvitationAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.RequestChangePassword:
                        await NotifyRequestPasswordChangeAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.ChangePassword:
                        await NotifyPasswordChangedAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.MfaTokenToPhone:
                        await NotifyMfaTokenToPhoneAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.MfaTokenToEmail:
                        await NotifyMfaTokenToEmailAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.ChangePhoneNumber:
                        await NotifyChangePhoneNumberAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.VerifyPhoneNumber:
                        await NotifyVerifiedPhoneNumberAsync(user, cancellationToken);
                        break;
                    case UserEventActivity.RemovePhoneNumber:
                        await NotifyRemovePhoneNumberAsync(user, cancellationToken);
                        break;
                }
            }
        }

        private async Task NotifyInvitationAsync(IUser user, CancellationToken cancellationToken)
        {
            string layout = await FileHelper.ReadEmailTemplateAsync("layout.html", cancellationToken);
            string invite = await FileHelper.ReadEmailTemplateAsync("partial\\invite.html", cancellationToken);

            var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);
            var link = $"{user.Link}/accounts/accept-invitation?email={user.Email}&token={token}";

            var emailContent = layout.Replace("{{SECTION_BODY}}", invite).Replace("{{CLIENT_URL}}", user.Link);
            emailContent = emailContent.Replace("{{LINK}}", link)
                                       .Replace("{{INVITER}}", user.LastUpdatedBy)
                                       .Replace("{{ROLE}}", user.RoleName);

            var emailModel = new EmailModel
            {
                Body = emailContent,
                To = user.Email,
                Subject = "OFFICESYNC Invitation"
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }

        private async Task NotifyAcceptInvitationAsync(IUser user, CancellationToken cancellationToken)
        {
            string layout = await FileHelper.ReadEmailTemplateAsync("layout.html", cancellationToken);
            string acceptInvitation = await FileHelper.ReadEmailTemplateAsync("partial\\accept-invitation.html", cancellationToken);

            var emailContent = layout.Replace("{{SECTION_BODY}}", acceptInvitation).Replace("{{CLIENT_URL}}", user.Link);
            emailContent = emailContent.Replace("{{LINK}}", user.Link);

            var emailModel = new EmailModel
            {
                Body = emailContent,
                To = user.Email,
                Subject = "OFFICESYNC Invitation Accepted"
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }

        private async Task NotifyRequestPasswordChangeAsync(IUser user, CancellationToken cancellationToken)
        {
            string layout = await FileHelper.ReadEmailTemplateAsync("layout.html", cancellationToken);
            string requestPasswordChange = await FileHelper.ReadEmailTemplateAsync("partial\\request-password-change.html", cancellationToken);

            var emailContent = layout.Replace("{{SECTION_BODY}}", requestPasswordChange).Replace("{{CLIENT_URL}}", user.Link);
            emailContent = emailContent.Replace("{{LINK}}", user.Link);

            var emailModel = new EmailModel
            {
                Body = emailContent,
                To = user.Email,
                Subject = "OFFICESYNC Password Change Request"
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }

        private async Task NotifyPasswordChangedAsync(IUser user, CancellationToken cancellationToken)
        {
            var emailModel = new EmailModel
            {
                Body = user.Link,
                To = user.Email,
                Subject = "OFFICESYNC Password Changed Successfully"
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }

        private async Task NotifyMfaTokenToPhoneAsync(IUser user, CancellationToken cancellationToken)
        {
            var message = $"Your security code is: {user.Token}";
            // Send SMS
            await _twilioService.SendSMSAsync(user.PhoneNumber, message);
        }

        private async Task NotifyMfaTokenToEmailAsync(IUser user, CancellationToken cancellationToken)
        {
            var message = $"Your security code is: {user.Token}";

            var emailModel = new EmailModel
            {
                Body = message,
                Subject = "OFFICESYNC MFA Token",
                To = user.Email
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }

        private async Task NotifyChangePhoneNumberAsync(IUser user, CancellationToken cancellationToken)
        {
            var message = $"Your security code is: {user.Token}";
            // Send SMS
            await _twilioService.SendSMSAsync(user.PhoneNumber, message);
        }

        private async Task NotifyVerifiedPhoneNumberAsync(IUser user, CancellationToken cancellationToken)
        {
            var message = $"Your phone number ({user.PhoneNumber}) has been verified.";
            // Send SMS
            await _twilioService.SendSMSAsync(user.PhoneNumber, message);
        }

        private async Task NotifyRemovePhoneNumberAsync(IUser user, CancellationToken cancellationToken)
        {
            var message = "Your existing phone number has been removed.";
            var emailModel = new EmailModel
            {
                Body = message,
                Subject = "OFFICESYNC Phone Number Removed",
                To = user.Email
            };

            await _emailService.SendAsync(emailModel, cancellationToken);
        }
    }
}
