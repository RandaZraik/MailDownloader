using MailDownloader.Mail.Contracts;
using Limilabs.Client.POP3;
using Limilabs.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailDownloader.Mail
{
    /// <summary>
    /// POP3 Client
    /// </summary>
    public class Pop3Client : IPop3Client
    {
        private Pop3 _client;
        private readonly static MailBuilder _defaultMailBuilder = new MailBuilder();

        public Pop3Client()
        {
        }

        public Pop3 Client => _client = _client ?? new Pop3();
        public bool IsConnected => _client?.Connected ?? false;

        public MailClientType MailClientType => MailClientType.POP3;

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
                    var supportsSTLS = Client.SupportedExtensions().Contains(Pop3Extension.STLS);
                    if (supportsSTLS)
                        await Client.StartTLSAsync();
                }
            }

            await Client.LoginAsync(info.Username, info.Password);
        }

        public async Task<List<object>> GetAllInboxMessagesIdsAsync()
        {
            var messagesId = await Client.GetAllAsync();
            return messagesId.Cast<object>().ToList();
        }

        public async Task<IMail> GetMessageHeadersByIdAsync(object messageId)
        {
            var headers = await Client.GetHeadersByUIDAsync((string)messageId);
            return _defaultMailBuilder.CreateFromEml(headers);
        }

        public async Task<IMail> GetMessageByIdAsync(object messageId)
        {
            var messageBytes = await Client.GetMessageByUIDAsync((string)messageId);
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
