namespace Auth.API.Domain.Dtos.Common
{
    public class EmailSendDto
    {
        public EmailSendDto()
        {
            To = new List<string>();
            //Cc = new List<string>();
            //AttachmentFiles = new List<IFormFile>();
        }
        public List<string> To { get; set; }
        //public List<string>? Cc { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        //internal string Code { get; set; } = string.Empty; 
        //public List<IFormFile>? AttachmentFiles { get; set; }
    }
}
