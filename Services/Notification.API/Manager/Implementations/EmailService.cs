using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Notification.API.Manager.Interfaces;
using Notification.API.Helper.Configuration;
using Notification.API.Domain.Dto.Common;

namespace Auth.WebAPI.Helper.EmailHelper
{
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _emailSettings;
        private readonly EmailSettingRetry _emailSettingsRetry;
        private readonly ILogger<EmailService> _logger;
        
        public EmailService(
            IOptionsSnapshot<EmailSetting> options,
            IOptionsSnapshot<EmailSettingRetry> optionsRetry,
            ILogger<EmailService> logger
            )
        {
            _emailSettings = options.Value;
            _emailSettingsRetry = optionsRetry.Value;
            _logger = logger;
        }
        public async Task<bool> SendEmailAsync(EmailDto dto)
        {
            var builder = new BodyBuilder();
            var email = new MimeMessage();
            try
            {
                email.Subject = dto.Subject;
                email.From.Add(MailboxAddress.Parse(_emailSettings.EmailFrom));
                dto.To.ForEach(x => email.To.Add(MailboxAddress.Parse(x)));

                if (dto.Cc != null && dto.Cc.Count != 0)
                    dto.Cc.ForEach(x => email.Cc.Add(MailboxAddress.Parse(x)));

                if (dto.AttachmentFiles != null && dto.AttachmentFiles.Count != 0)
                {
                    foreach (var attachmentFile in dto.AttachmentFiles)
                    {
                        using var attachmentStream = attachmentFile.OpenReadStream();
                        var attachment = new MimePart()
                        {
                            Content = new MimeContent(attachmentStream),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Path.GetFileName(attachmentFile.FileName)
                        };

                        email.Attachments?.Append(attachment);
                        await builder.Attachments.AddAsync(attachmentFile.FileName, attachmentStream);
                    }
                }

                builder.HtmlBody = dto.Body ?? string.Empty;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpHost, Convert.ToInt32(_emailSettings.SmtpPort), SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
                email.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    using var smtp = new SmtpClient();
                    await smtp.ConnectAsync(_emailSettingsRetry.SmtpHost, Convert.ToInt32(_emailSettingsRetry.SmtpPort), SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettingsRetry.SmtpUser, _emailSettingsRetry.SmtpPass);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    email.Dispose();
                    return true;
                }
                catch (Exception exc)
                {
                    _logger.LogError(exc, "{Message} Inner exception: {InnerException}", ex.Message, ex.InnerException);
                    return false;
                }
            }
        }
    }
}
