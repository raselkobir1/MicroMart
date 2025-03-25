using Notification.API.Helper.Enum;

namespace Notification.API.Domain.Dto.Common
{
    public class SendEmailDto
    {
        public List<string> To { get; set; } = [];
        public List<string>? Cc { get; set; } = [];
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? Base64FileName { get; set; }
        public string? Base64Data { get; set; }
        public IFormFile? AttachmentFile { get; set; }
    }

    public class EmailNotificationSendDto
    {
        public MailSender MailSender { get; set; } = MailSender.SystemMail;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> To { get; set; } = [];
        public List<string> Cc { get; set; } = [];
        public List<EmailAttachmentDto> AttachmentFiles { get; set; } = [];
    }

    public class EmailAttachmentDto
    {
        public string FileName { get; set; } = string.Empty;
        public string FileBase64 { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
        public string MediaSubType { get; set; } = string.Empty;
    }

    public static class EmailAttachmentDtoConvertor
    {
        public static EmailNotificationSendDto ConvertToEmailNotificationSendDto(this SendEmailDto emailDto)
        {
            var emailNotificationDto = new EmailNotificationSendDto
            {
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                To = emailDto.To,
                Cc = emailDto.Cc ?? [],
            };

            if (!string.IsNullOrEmpty(emailDto.Base64Data))
            {
                emailNotificationDto.AttachmentFiles.Add(new EmailAttachmentDto
                {
                    FileBase64 = emailDto.Base64Data,
                    FileName = emailDto.Base64FileName ?? "Attachment.pdf",
                    MediaType = "application",
                    MediaSubType = "pdf"
                });
            }

            if (emailDto.AttachmentFile != null && emailDto.AttachmentFile.Length > 0)
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    emailDto.AttachmentFile.CopyTo(memoryStream);
                    emailNotificationDto.AttachmentFiles.Add(new EmailAttachmentDto
                    {
                        FileBase64 = Convert.ToBase64String(memoryStream.ToArray()),
                        FileName = emailDto.AttachmentFile.FileName ?? "Attachment",
                        MediaType = emailDto.AttachmentFile.ContentType?.Split('/')?.FirstOrDefault() ?? "application",
                        MediaSubType = emailDto.AttachmentFile.ContentType?.Split('/')?.ElementAtOrDefault(1) ?? "octet-stream"
                    });
                }
                catch { }
            }

            return emailNotificationDto;
        }
    }
}
