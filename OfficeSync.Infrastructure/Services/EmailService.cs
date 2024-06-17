using Microsoft.Extensions.Options;
using OfficeSync.Application.Common.Interfaces;
using OfficeSync.Application.Common.Models;
using OfficeSync.Infrastructure.Common.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace OfficeSync.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISendGridClient _sendGridClient;
        private readonly EmailOptions _emailConfiguration;

        public EmailService(ISendGridClient sendGridClient,
                            IOptions<EmailOptions> emailConfiguration)
        {
            _sendGridClient = sendGridClient;
            _emailConfiguration = emailConfiguration.Value;
        }

        public async Task SendAsync(EmailModel email, CancellationToken cancellationToken)
        {
            var hostEmail = string.IsNullOrEmpty(email.From) ? _emailConfiguration.HostEmail : email.From;

            EmailAddress from = new EmailAddress(hostEmail);
            EmailAddress recipient = new EmailAddress(email.To);
            string subject = string.IsNullOrEmpty(email.Subject) ? "OFFICESYNC" : email.Subject;

            var sendGridMessage = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                HtmlContent = email.Body,
                Personalizations = new List<Personalization>
                {
                    new Personalization
                    {
                        Ccs = email.Ccs?.Select(s => new EmailAddress(s)).ToList()
                    }
                }
            };

            sendGridMessage.AddTo(recipient);

            Response response = await _sendGridClient.SendEmailAsync(sendGridMessage, cancellationToken).ConfigureAwait(false);

            if (response != null && response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                Console.WriteLine($"Email not sent. Status >>> {response.StatusCode}");
            }
        }
    }
}
