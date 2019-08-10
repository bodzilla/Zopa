using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zopa.Core.Common;
using Zopa.Core.Contracts;
using Zopa.Core.Repositories;
using Zopa.Core.Services;
using Zopa.Models;

namespace Zopa.Console
{
    public class Program
    {
        private static string _path;
        private static int _amountRequested;

        private static void Main(string[] args)
        {
            _path = args[0];
            _amountRequested = int.Parse(args[1]);

            // Configure settings and dependencies.
            var serviceCollection = ConfigureServices();

            // Get application instance.
            App app;
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                app = serviceProvider.GetRequiredService<App>();
            }

            // Entry point for application.
            app.Run(_amountRequested);
        }

        /// <summary>
        /// Builds configuration builder and the IoC container with all dependencies.
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            // Set up the objects we need to get to configuration settings.
            var configuration = ConfigureSettings();

            services
                .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
                .AddSingleton(configuration)
                .AddTransient<IDataStore, DataStore>(serviceProvider => new DataStore(serviceProvider.GetRequiredService<ILogger<DataStore>>(), _path))
                .AddTransient<IRepository<Lender>, LenderRepository>()
                .AddTransient<IConditionService, ConditionService>()
                .AddTransient<IRepaymentService, RepaymentService>()
                .AddTransient<ILenderService, LenderService>()
                .AddTransient<IQuoteService, QuoteService>()
                .AddTransient<App>();

            return services;
        }

        /// <summary>
        /// Builds the configuration settings.
        /// </summary>
        /// <returns></returns>
        private static IConfiguration ConfigureSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            var configuration = builder.Build();
            return configuration;
        }
    }
}
