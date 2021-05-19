namespace MailDownloader.Mail.Contracts
{
    /// <summary>
    /// Mail Encryption Types
    /// </summary>
    public enum EncryptionType
    {
        /// <summary>
        /// Unencrypted
        /// </summary>
        Unencrypted = 0,

        /// <summary>
        /// SSL/TLS
        /// </summary>
        SslTls = 1,

        /// <summary>
        /// STARTTLS
        /// </summary>
        StartTls = 2,
    }
}
