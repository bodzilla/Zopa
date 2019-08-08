using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Zopa.Core.Common;
using Zopa.Core.Contracts;
using Zopa.Core.Services;

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

            // Configure dependencies.
            ServiceProvider serviceProvider;
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Get application instance.
            App app;
            using (serviceProvider = serviceCollection.BuildServiceProvider())
            {
                app = serviceProvider.GetRequiredService<App>();
            }

            // Entry point for application.
            app.Run(_path, _amountRequested);
        }

        /// <summary>
        /// Builds the IoC container with all dependencies.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        private static void ConfigureServices(IServiceCollection serviceCollection)
            => serviceCollection
                .AddLogging(x => x.AddConsole())
                .AddScoped<IDataStore, CsvStore>(serviceProvider =>
                    new CsvStore(serviceProvider.GetRequiredService<ILogger<CsvStore>>(), _path))
                .AddScoped<IRepaymentCalculator, RepaymentCalculator>()
                .AddScoped<IRepaymentService, RepaymentService>()
                .AddScoped<ILenderService, LenderService>()
                .AddScoped<IQuoteService, QuoteService>()
                .AddSingleton<App>();
    }
}
