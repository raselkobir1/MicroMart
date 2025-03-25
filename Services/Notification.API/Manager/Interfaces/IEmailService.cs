using Notification.API.Domain.Dto.Common;

namespace Notification.API.Manager.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailDto dto);
    }
}
