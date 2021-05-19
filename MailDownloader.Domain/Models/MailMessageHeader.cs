using System;

namespace MailDownloader.Domain.Models
{
    /// <summary>
    /// The Mail Message's headers
    /// </summary>
    public class MailMessageHeader
    {
        /// <summary>
        /// The message Id
        /// </summary>
        public object MessageId { get; set; }

        /// <summary>
        /// The message subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The sender email
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// The receiver email
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// The message send date
        /// </summary>
        public DateTime? SendDate { get; set; }
    }
}
