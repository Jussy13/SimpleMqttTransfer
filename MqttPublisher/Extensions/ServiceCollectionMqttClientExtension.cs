using System;
using Microsoft.Extensions.DependencyInjection;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MqttPublisher.Services;
using MqttPublisher.Settings;

namespace MqttPublisher.Extensions
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
                                    .WithTopic("/publisher_will_topic")
                                    .WithPayload("publisher_will_message")
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

            return services;
        }
    }
}
