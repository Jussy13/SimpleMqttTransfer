using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MqttPublisher.Extensions;
using MqttPublisher.Settings;


namespace MqttPublisher
{
    public class Startup
    {
        private void MapConfiguration(IConfiguration configuration)
        {
            MapBrokerHostSettings(configuration);
            MapClientSettings(configuration);
        }

        private void MapBrokerHostSettings(IConfiguration configuration)
        {
            BrokerHostSettings brokerHostSettings = new BrokerHostSettings();
            configuration.GetSection(nameof(BrokerHostSettings)).Bind(brokerHostSettings);
            AppSettingsProvider.BrokerHostSettings = brokerHostSettings;
        }

        private void MapClientSettings(IConfiguration configuration)
        {
            ClientSettings clientSettings = new ClientSettings();
            configuration.GetSection(nameof(ClientSettings)).Bind(clientSettings);
            AppSettingsProvider.ClientSettings = clientSettings;
        }

        public IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            MapConfiguration(configuration);

            return services.AddMqttClientHostedService();
        }
    }
}
