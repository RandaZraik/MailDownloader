namespace MailDownloader.Mail.Contracts
{
    /// <summary>
    /// Mail Connection Information
    /// </summary>
    public class MailConnectionInfo
    {
        /// <summary>
        /// Mail client type
        /// </summary>
        public MailClientType ClientType { get; set; }

        /// <summary>
        /// Encryption type
        /// </summary>
        public EncryptionType EncryptionType { get; set; }

        /// <summary>
        /// Server name
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Mail's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Mail's password
        /// </summary>
        public string Password { get; set; }
    }
}
