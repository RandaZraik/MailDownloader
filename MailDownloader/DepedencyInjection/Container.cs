using MailDownloader.Domain;
using MailDownloader.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MailDownloader.DepedencyInjection
{
    public static class Container
    {
        private static IServiceProvider _serviceProvider;
 
        static Container()
        {
            ConfigureServices();
        }

        public static IServiceProvider ServiceProvider => _serviceProvider;

        private static void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAppDomain();
            serviceCollection.AddAppServices();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
