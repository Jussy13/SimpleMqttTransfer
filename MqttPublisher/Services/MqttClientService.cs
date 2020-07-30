using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace MqttPublisher.Services
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

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs) =>
            await Task.Run(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(eventArgs.ApplicationMessage.Topic))
                    {
                        return;
                    }

                    var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"+ Topic = {eventArgs.ApplicationMessage.Topic}");
                    Console.WriteLine($"+ Payload = {payload}");
                    Console.WriteLine($"+ QoS = {eventArgs.ApplicationMessage.QualityOfServiceLevel}");
                    Console.WriteLine($"+ Retain = {eventArgs.ApplicationMessage.Retain}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("connected");
            return _mqttClient.SubscribeAsync(
                new MqttTopicFilter[]
            {
                new MqttTopicFilter()
                {
                    Topic = "/client",
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                }
            });
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs) =>
            await Task.Run(() => Console.WriteLine("disconnected"));


        public Task StartAsync() => _mqttClient.StartAsync(_options);

        public Task StopAsync() => _mqttClient.StopAsync();

        public Task PublishAsync(
            string payload,
            string topic = "/publisher",
            bool retainFlag = false,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce
        ) =>
            _mqttClient.PublishAsync(
                new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel(qos)
                    .WithRetainFlag(retainFlag)
                    .Build()
            );

        public Task SubscribeAsync(
            string topic,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce
        ) =>
            _mqttClient.SubscribeAsync(
                new MqttTopicFilter()
                {
                    Topic = topic,
                    QualityOfServiceLevel = qos
                }
            );
    }
}
