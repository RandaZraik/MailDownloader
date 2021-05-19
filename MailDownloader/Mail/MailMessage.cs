using System;

namespace MailDownloader
{
    public class MailMessage : BindableBase
    {
        public object Id { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string BodyHtml { get; set; }
        public DateTime? SendDate { get; set; }
    }
}
