using MailDownloader.Mail.Contracts;
using System;
namespace MailDownloader.Mail
{
    public static class MailClientFactory
    {
        /// <summary>
        /// Creates the mail client based on the type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IMailClient CreateMailClient(MailClientType type)
        {
            switch (type)
            {
                case MailClientType.IMAP:
                    return new ImapClient();
                case MailClientType.POP3:
                    return new Pop3Client();
                default:
                    throw new Exception($"Unsupported client type {type}");
            }
        }
    }
}
