namespace MailDownloader.Domain.Models
{
    /// <summary>
    /// The Mail Message's body - HTML/Text
    /// </summary>
    public class MailMessageBody
    {
        /// <summary>
        /// The message Id
        /// </summary>
        public object MessageId { get; set; }

        /// <summary>
        /// The HTML body for the message
        /// </summary>
        public string BodyHtml { get; set; }

        /// <summary>
        /// The text body for the message
        /// </summary>
        public string BodyText { get; set; }
    }
}
