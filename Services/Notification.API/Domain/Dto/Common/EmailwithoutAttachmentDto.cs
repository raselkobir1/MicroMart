namespace Notification.API.Domain.Dto.Common
{
    public class EmailwithoutAttachmentDto
    {
        public EmailwithoutAttachmentDto()
        {
            To = new List<string>();
            Cc = new List<string>();
        }
        public List<string> To { get; set; } 
        public List<string>? Cc { get; set; } 
        public string Subject { get; set; } = string.Empty;   
        public string Body { get; set; } = string.Empty;  
    }
}
