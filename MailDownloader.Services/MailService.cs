using AutoMapper;
using MailDownloader.Domain.Models;
using MailDownloader.Domain.Services;
using MailDownloader.Domain.Threading;
using MailDownloader.Mail;
using MailDownloader.Mail.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailDownloader.Services
{
    internal class MailService : IMailService
    {
        private readonly IMapper _mapper;

        public int MaxNumberOfConcurrentThreads { get; set; } = 5;

        public MailService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task DownloadMessageBodyAsync(MailConnectionInfo connectionInfo,
            object messageId,
            Action<MailMessageBody> onEmailBodyReceiver,
            Func<object, bool> isBodyAlreadyDownloaded)
        {
            await Task.Run(async () =>
            {
                using (var client = MailClientFactory.CreateMailClient(connectionInfo.ClientType))
                {
                    await client.ConnectAsync(connectionInfo);
                    await DownloadMailMessageBodyAsync(client, messageId, onEmailBodyReceiver, isBodyAlreadyDownloaded);
                }
            });
        }

        public async Task DownloadAllMailsAsync(MailConnectionInfo connectionInfo,
            Action<MailMessageHeader> onEmailHeadersReceiver,
            Action<MailMessageBody> onEmailBodiesReceiver,
            Func<object, bool> isBodyAlreadyDownloaded)
        {
            // Get all messages ids.
            var messageIds = (List<object>)null;
            using (var client = MailClientFactory.CreateMailClient(connectionInfo.ClientType))
            {
                await client.ConnectAsync(connectionInfo);
                messageIds = await client.GetAllInboxMessagesIdsAsync();
            }

            var tasks = new List<Task>();
            var messageIdsQueue = new ConcurrentQueue<object>(messageIds);

            // Create 5 threads and a connection for each.
            for (var i = 0; i < MaxNumberOfConcurrentThreads; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var client = MailClientFactory.CreateMailClient(connectionInfo.ClientType))
                    {
                        await client.ConnectAsync(connectionInfo);
                        while (messageIdsQueue.TryDequeue(out object messageId))
                        {
                            // Get the message body
                            await DownloadMailMessageBodyAsync(client, messageId, onEmailBodiesReceiver, isBodyAlreadyDownloaded);
                            // Get the message headers
                            var header = _mapper.Map<MailMessageHeader>(await client.GetMessageHeadersByIdAsync(messageId));
                            header.MessageId = messageId;
                            onEmailHeadersReceiver(header);
                        }
                    }
                }));
            }

            await Task.WhenAll(tasks);
        }

        private async Task DownloadMailMessageBodyAsync(IMailClient client,
            object messageId,
            Action<MailMessageBody> onEmailBodiesReceiver,
            Func<object, bool> isBodyAlreadyDownloaded)
        {
            var locker = new KeyedLock<string>();
            var lockKey = messageId.ToString();

            await locker.WaitAsync(lockKey);
            try
            {
                if (!isBodyAlreadyDownloaded(messageId))
                {
                    var body = _mapper.Map<MailMessageBody>(await client.GetMessageByIdAsync(messageId));
                    body.MessageId = messageId;
                    if (!isBodyAlreadyDownloaded(messageId))
                        onEmailBodiesReceiver(body);
                }
            }
            finally
            {
                locker.Release(lockKey);
            }
        }
    }
}