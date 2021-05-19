using Microsoft.Extensions.DependencyInjection;

namespace MailDownloader.Domain
{
    /// <summary>
    /// Extensions for the service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddAppDomain(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
        }
    }
}