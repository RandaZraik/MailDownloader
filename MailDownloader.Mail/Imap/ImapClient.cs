using MailDownloader.Mail.Contracts;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailDownloader.Mail
{
    /// <summary>
    /// IMAP Client
    /// </summary>
    public class ImapClient : IImapClient
    {
        private Imap _client;
        private readonly static MailBuilder _defaultMailBuilder = new MailBuilder();

        public ImapClient()
        {
        }

        public Imap Client => _client = _client ?? new Imap();
        public bool IsConnected => _client?.Connected ?? false;

        public MailClientType MailClientType => MailClientType.IMAP;

        public async Task ConnectAsync(MailConnectionInfo info)
        {
            if (info.EncryptionType == EncryptionType.SslTls)
            {
                await Client.ConnectSSLAsync(info.Server, info.Port);
            }
            else
            {
                await Client.ConnectAsync(info.Server, info.Port);

                if (info.EncryptionType == EncryptionType.StartTls)
                {
                    var supportsStartTLS = Client.SupportedExtensions().Contains(ImapExtension.StartTLS);
                    if (supportsStartTLS)
                        await Client.StartTLSAsync();
                }
            }

            await Client.UseBestLoginAsync(info.Username, info.Password);
            await Client.SelectInboxAsync();
        }

        public async Task<List<object>> GetAllInboxMessagesIdsAsync()
        {
            var messagesIds = await Client.SearchAsync(Flag.All);
            return messagesIds.Cast<object>().ToList();
        }

        public async Task<IMail> GetMessageHeadersByIdAsync(object messageId)
        {
            var headers = await Client.GetHeadersByUIDAsync((long)messageId);
            return _defaultMailBuilder.CreateFromEml(headers);
        }

        public async Task<IMail> GetMessageByIdAsync(object messageId)
        {
            var messageBytes = await Client.GetMessageByUIDAsync((long)messageId);
            return _defaultMailBuilder.CreateFromEml(messageBytes);
        }

        public async Task DisconnectAsync()
        {
            Client.Dispose();
            await Client.CloseAsync();

            _client = null;
        }

        public async void Dispose()
        {
            await DisconnectAsync();
        }
    }
}
