using MailDownloader.Mail.Contracts;
using Limilabs.Mail;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailDownloader.Mail
{
    /// <summary>
    /// The interface for Mail Clients
    /// </summary>
    public interface IMailClient : IDisposable
    {
        /// <summary>
        /// Mail client type
        /// </summary>
        MailClientType MailClientType { get; }

        /// <summary>
        /// Is the mail client connection open
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connect to the mail server
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Task ConnectAsync(MailConnectionInfo info);

        /// <summary>
        /// Disconnect to the mail server
        /// </summary>
        /// <returns></returns>
        Task DisconnectAsync();

        /// <summary>
        /// Get all mail messages ids
        /// </summary>
        /// <returns></returns>
        Task<List<object>> GetAllInboxMessagesIdsAsync();

        /// <summary>
        /// Get the mail headers
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<IMail> GetMessageHeadersByIdAsync(object messageId);

        /// <summary>
        /// Get the mail envelope
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<IMail> GetMessageByIdAsync(object messageId);
    }
}
