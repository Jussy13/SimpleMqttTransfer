using System;
using System.Collections.Generic;
using System.Linq;
using EventsModeling.Models.Transactions;
using EventsModeling.Services;
using EventsModeling.Services.Handlers;
using EventsModeling.Services.Transactions;
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
            var selection = configuration.GetSection("Props");

            if (!selection.Exists())
                throw new Exception("Props does not exist");

            var children = selection.GetChildren();

            foreach (var subSection in children)
            {
                var creator = new TransactionSettings();
                subSection.Bind(creator);

                if (!TransactionHelper.SettingsByType.ContainsKey(subSection.Key))
                    TransactionHelper.SettingsByType.Add(subSection.Key, creator);
            }
        }

        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            ConfigSettings(configuration);

            var totalFactories = (double)TransactionHelper.SettingsByType.Sum(settings => settings.Value.FactoriesCount);

            var freqByType = new Dictionary<string, double>();
            var totalFreq = 0.0;

            foreach (var settings in TransactionHelper.SettingsByType)
            {
                var count = settings.Value.FactoriesCount / totalFactories;
                var freq = Math.Round(count, 3);
                totalFreq += freq;
                freqByType.Add(settings.Key, freq);
            }


            if (totalFreq > 1.0)
            {
                freqByType = new Dictionary<string, double>(freqByType.OrderByDescending(f => f.Value));
                var item = freqByType.Last();
                freqByType[item.Key] = item.Value + (totalFreq - 1.0);
            }

            TransactionHelper.FreqByType = freqByType;

            for (var i = 1; i <= AppSettingsProvider.CoresCount; i++)
            for (var j = 1; j <= AppSettingsProvider.CoresCount; j++)
                TransactionHelper.RequiredCoresSets.Add(new List<int>(2) { i, j });

            services
                .AddSingleton<IEventHandler, EventHandlerWrapper>()
                .AddSingleton<Executor>();

            return services;
        }
    }
}
