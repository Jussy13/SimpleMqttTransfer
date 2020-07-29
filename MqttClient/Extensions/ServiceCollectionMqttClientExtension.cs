using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MqttClient.Services;
using MqttClient.Settings;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Client.Extensions
{
    public static class ServiceCollectionMqttClientExtension
    {
        public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services)
        {
            services.AddSingleton<IManagedMqttClientOptions>(serviceProvider =>
            {
                var clientSettings = AppSettingsProvider.ClientSettings;
                var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;

                var optionsBuilder = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(
                        new MqttClientOptionsBuilder()
                            .WithCredentials(clientSettings.UserName, clientSettings.Password)
                            .WithClientId(clientSettings.Id)
                            .WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port)
                            .WithWillMessage(
                                new MqttApplicationMessageBuilder()
                                    .WithTopic("/client_will_topic")
                                    .WithPayload("client_will_message")
                                    .WithRetainFlag(false)
                                    .WithExactlyOnceQoS()
                                    .Build()
                            )
                            .WithCleanSession()
                            .WithKeepAlivePeriod(TimeSpan.FromSeconds(60))
                            .Build()
                    );

                return optionsBuilder.Build();
            });
            services.AddSingleton<IMqttClientService, MqttClientService>();
            services.AddSingleton<IHostedService>(serviceProvider => serviceProvider.GetService<IMqttClientService>());

            return services;
        }
    }
}
