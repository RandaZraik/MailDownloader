using MailDownloader.DepedencyInjection;
using MailDownloader.Domain.Services;
using MailDownloader.Mail.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;

namespace MailDownloader
{
    public class MainWindowViewModel : ValidatableBindableBase
    {
        private readonly IMailService _mailService;

        private string _portNumber;
        private string _server;
        private string _username;
        private string _password;
        private string _mailBodyText;
        private string _updateMessage;
        private LookupModel _mailClientType;
        private LookupModel _encryptionType;
        private ObservableCollection<MailMessage> _mailMessages;
        private Dictionary<object, string> _mailMessagesBodies;
        private MailMessage _selectMailMessage;
        private bool _inProcess;

        public MainWindowViewModel()
        {
            _mailService = Container.ServiceProvider.GetRequiredService<IMailService>();
            _mailMessages = new ObservableCollection<MailMessage>();
            _mailMessagesBodies = new Dictionary<object, string>();
            StartCommand = new RelayCommand(OnStart, CanStart);
            ErrorsChanged += (s, e) => StartCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand StartCommand { get; }

        [Required]
        public string Port
        {
            get => _portNumber;
            set => SetProperty(ref _portNumber, value);
        }

        [Required]
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        [Required]
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        [Required]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ObservableCollection<MailMessage> MailMessages
        {
            get => _mailMessages;
            set => SetProperty(ref _mailMessages, value);
        }

        [Required]
        public LookupModel ClientType
        {
            get => _mailClientType;
            set => SetProperty(ref _mailClientType, value);
        }

        [Required]
        public LookupModel Encryption
        {
            get => _encryptionType;
            set => SetProperty(ref _encryptionType, value);
        }

        public string MailBodyText
        {
            get => _mailBodyText;
            set => SetProperty(ref _mailBodyText, value);
        }

        public string UpdateMessage
        {
            get => _updateMessage;
            set => SetProperty(ref _updateMessage, value);
        }

        public bool InProcess
        {
            get => _inProcess;
            set => SetProperty(ref _inProcess, value);
        }

        public List<LookupModel> MailClientTypes => new List<LookupModel>
        {
            new LookupModel((int)MailClientType.IMAP, "IMAP"),
            new LookupModel((int)MailClientType.POP3, "POP3"),
        };

        public List<LookupModel> EncryptionTypes => new List<LookupModel>
        {
            new LookupModel((int)EncryptionType.Unencrypted, "Unencrypted"),
            new LookupModel((int)EncryptionType.SslTls, "SSL/TLS"),
            new LookupModel((int)EncryptionType.StartTls, "STARTTLS"),
        };

        public MailMessage SelectMailMessage
        {
            get => _selectMailMessage;
            set
            {
                SetProperty(ref _selectMailMessage, value);

                if (value == null)
                    return;
                if (_mailMessagesBodies.ContainsKey(value.Id))
                    MailBodyText = _mailMessagesBodies[value.Id];
                else
                {
                    _mailService.DownloadMessageBodyAsync(
                        GetConnectionInfo(),
                        value.Id,
                        (body) => Application.Current.Dispatcher.Invoke(() =>
                        {
                            _mailMessagesBodies.Add(value.Id, body.BodyHtml);
                            MailBodyText = body.BodyHtml;
                        }),
                        (id) => _mailMessagesBodies.ContainsKey(id));
                }
            }
        }

        private bool CanStart() => !HasErrors && !InProcess;

        private async void OnStart()
        {
            if (!IsValidModel())
                return;

            InProcess = true;
            await LoadMessagesAsync();
            InProcess = false;
        }

        private bool IsValidModel()
        {
            Username = Username; Password = Password; Port = Port; Server = Server; ClientType = ClientType; Encryption = Encryption;
            return !HasErrors;
        }

        private async Task LoadMessagesAsync()
        {
            MailMessages.Clear();
            _mailMessagesBodies.Clear();
            MailBodyText = null;
            UpdateMessage = null;

            try
            {
                UpdateMessage = "Downloading mail headers and texts...";

                await _mailService.DownloadAllMailsAsync(
                    GetConnectionInfo(),
                    (header) => Application.Current.Dispatcher.Invoke(() => MailMessages.Add(new MailMessage
                    {
                        Id = header.MessageId,
                        From = header.From,
                        Subject = header.Subject,
                        SendDate = header.SendDate,
                    })),
                    (body) => Application.Current.Dispatcher.Invoke(() => _mailMessagesBodies.Add(body.MessageId, body.BodyHtml)),
                    (id) => _mailMessagesBodies.ContainsKey(id));

                UpdateMessage = "Download is complete";
            }
            catch (Exception ex)
            {
                UpdateMessage = ex.Message;
            }
        }

        private MailConnectionInfo GetConnectionInfo() => new MailConnectionInfo
        {
            ClientType = (MailClientType)ClientType.Id,
            EncryptionType = (EncryptionType)Encryption.Id,
            Server = Server,
            Port = int.TryParse(Port, out var port) ? port : -1,
            Username = Username,
            Password = Password,
        };
    }
}
