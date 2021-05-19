using MailDownloader.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MailDownloader.Services
{
    /// <summary>
    /// Extensions for the service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService>();
        }
    }
}
