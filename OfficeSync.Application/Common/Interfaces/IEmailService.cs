using OfficeSync.Application.Common.Models;

namespace OfficeSync.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailModel email, CancellationToken cancellationToken);
    }
}
