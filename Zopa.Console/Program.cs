using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            app.Run(_amountRequested);
        }

        /// <summary>
        /// Builds the IoC container with all dependencies.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        private static void ConfigureServices(IServiceCollection serviceCollection)
            => serviceCollection
                .AddLogging(x => x.AddConsole())
                .AddScoped<IRepository<Lender>, LenderRepository>(serviceProvider =>
                    new LenderRepository(serviceProvider.GetRequiredService<ILogger<LenderRepository>>(), _path))
                .AddScoped<IConditionService, ConditionService>()
                .AddScoped<IRepaymentService, RepaymentService>()
                .AddScoped<ILenderService, LenderService>()
                .AddScoped<IQuoteService, QuoteService>()
                .AddSingleton<App>();
    }
}
