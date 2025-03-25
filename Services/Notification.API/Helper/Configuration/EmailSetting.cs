using System.ComponentModel.DataAnnotations;

namespace Notification.API.Helper.Configuration
{
    public class EmailSetting
    {
        public string RefreshTokenTTL { get; set; }


        [Required(AllowEmptyStrings = false)]
        public string EmailFrom { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SmtpHost { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SmtpPort { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SmtpUser { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string SmtpPass { get; set; }
    }
}
