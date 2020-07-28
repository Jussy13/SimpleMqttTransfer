﻿using System;
using System.Text;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;

namespace MqttClient.Services
{
    public class MqttClientService : IMqttClientService
    {
        private readonly IManagedMqttClient _mqttClient;
        private readonly IManagedMqttClientOptions _options;

        public MqttClientService(IManagedMqttClientOptions options)
        {
            _options = options;
            _mqttClient = new MqttFactory().CreateManagedMqttClient();
            ConfigureMqttClient();
        }

        private void ConfigureMqttClient()
        {
            _mqttClient.ConnectedHandler = this;
            _mqttClient.DisconnectedHandler = this;
            _mqttClient.ApplicationMessageReceivedHandler = this;
        }

        public Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            return Task.Run(() =>
            {
                try
                {
                    string topic = eventArgs.ApplicationMessage.Topic;
                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                        Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                        Console.WriteLine($"+ Topic = {eventArgs.ApplicationMessage.Topic}");
                        Console.WriteLine($"+ Payload = {payload}");
                        Console.WriteLine($"+ QoS = {eventArgs.ApplicationMessage.QualityOfServiceLevel}");
                        Console.WriteLine($"+ Retain = {eventArgs.ApplicationMessage.Retain}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });
        }

        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("connected");
            await _mqttClient.SubscribeAsync(
                new MqttTopicFilter[]
                {
                    new MqttTopicFilter()
                    {
                        Topic = "/publisher",
                        QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
                    },
                    new MqttTopicFilter()
                    {
                        Topic = "/publisher_will_topic",
                        QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
                    },
                });
        }

        public Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            return Task.Run(() => Console.WriteLine("disconnected"));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _mqttClient.StartAsync(_options);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                await _mqttClient.StopAsync();
            }
        }

        public async Task PublishAsync(string payload, string topic = "/client", bool retainFlag = false, int qos = 1) =>
            await _mqttClient.PublishAsync(
                new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                    .WithRetainFlag(retainFlag)
                    .Build()
            );

        public async Task SubscribeAsync(string topic, int qos = 1) =>
            await _mqttClient.SubscribeAsync(
                new MqttTopicFilter()
                {
                    Topic = topic,
                    QualityOfServiceLevel = (MQTTnet.Protocol.MqttQualityOfServiceLevel)qos
                }
            );
    }
}