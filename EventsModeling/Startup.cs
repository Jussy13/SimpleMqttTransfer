using System;
using EventsModeling.Resources;
using EventsModeling.Services.Handlers;
using EventsModeling.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventsModeling
{
    public static class Startup
    {
        private static void ConfigSettings(IConfiguration configuration)
        {
            configuration.GetSection("Settings").Bind(new AppSettingsProvider());
            Console.WriteLine();
        }

        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            ConfigSettings(configuration);
            ServerResources.InitServerResources(AppSettingsProvider.CoresCount, AppSettingsProvider.RamCount);

            services
                .AddSingleton<IEventHandler, EventHandlerWrapper>()
                .AddSingleton<Executor>();

            return services;
        }
    }
}
