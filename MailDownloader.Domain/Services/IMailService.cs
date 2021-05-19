using MailDownloader.Domain.Models;
using MailDownloader.Mail.Contracts;
using System;
using System.Threading.Tasks;

namespace MailDownloader.Domain.Services
{
    /// <summary>
    /// Interface for the Mail Service
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Downloads the mail message body.
        /// </summary>
        /// <param name="connectionInfo">Mail server information</param>
        /// <param name="messageId">The message id</param>
        /// <param name="onEmailBodyReceiver">The action that needs to be run against the downloaded emails bodies</param>
        /// <param name="isBodyAlreadyDownloaded">The func that checks if the message body already downloaded</param>
        /// <returns></returns>
        Task DownloadMessageBodyAsync(MailConnectionInfo connectionInfo,
            object messageId,
            Action<MailMessageBody> onEmailBodyReceiver,
            Func<object, bool> isBodyAlreadyDownloaded);

        /// <summary>
        /// Downloads all mail messages in the inbox.
        /// </summary>
        /// <param name="connectionInfo">Mail server information</param>
        /// <param name="onEmailHeadersReceiver">The action that needs to be run against the downloaded emails headers</param>
        /// <param name="onEmailBodiesReceiver">The action that needs to be run against the downloaded emails bodies</param>
        /// <param name="isBodyAlreadyDownloaded">The func that checks if the message body already downloaded</param>
        /// <returns></returns>
        Task DownloadAllMailsAsync(MailConnectionInfo connectionInfo,
            Action<MailMessageHeader> onEmailHeadersReceiver,
            Action<MailMessageBody> onEmailBodiesReceiver,
            Func<object, bool> isBodyAlreadyDownloaded);
    }
}
