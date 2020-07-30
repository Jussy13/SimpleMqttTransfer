using System;
using System.Text;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using System.Threading;
using System.Threading.Tasks;
using Database.Models;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;

namespace MqttClient.Services
{
    public class MqttClientService : IMqttClientService
    {
        private readonly IManagedMqttClient _mqttClient;
        private readonly IManagedMqttClientOptions _options;
        private readonly CalculationContext _dbContext;
        private readonly HttpTransportService _httpClient;

        public MqttClientService(
            IManagedMqttClientOptions options,
            CalculationContext dbContext,
            HttpTransportService httpClient
        )
        {
            _options = options;
            _dbContext = dbContext;
            _httpClient = httpClient;
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
            await Task.Run(new Func<Task>(async () =>
            {
                try
                {
                    string topic = eventArgs.ApplicationMessage.Topic;
                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        var payload = Encoding.UTF8.GetString(eventArgs.ApplicationMessage.Payload);
                        var qos = eventArgs.ApplicationMessage.QualityOfServiceLevel;
                        var retain = eventArgs.ApplicationMessage.Retain;
                        Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                        Console.WriteLine($"+ Topic = {topic}");
                        Console.WriteLine($"+ Payload = {payload}");
                        Console.WriteLine($"+ QoS = {qos}");
                        Console.WriteLine($"+ Retain = {retain}");

                        var message = new Message()
                        {
                            ReceivedAt = DateTime.Now,
                            Topic = topic,
                            Payload = payload,
                            Qos = (uint)qos,
                            Retain = retain
                        };
                        await _httpClient.SendMessageAsync(message);
                        await _dbContext.Messages.AddAsync(message);
                        await _dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            }));


        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            Console.WriteLine("connected");
            return _mqttClient.SubscribeAsync(
                new MqttTopicFilter[]
                {
                    new MqttTopicFilter()
                    {
                        Topic = "/publisher",
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                    },
                    new MqttTopicFilter()
                    {
                        Topic = "/publisher_will_topic",
                        QualityOfServiceLevel = MqttQualityOfServiceLevel.ExactlyOnce
                    },
                });
        }

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs) =>
            await  Task.Run(() => Console.WriteLine("disconnected"));

        public Task StartAsync() => _mqttClient.StartAsync(_options);

        public Task StartAsync(CancellationToken cancellationToken) => _mqttClient.StartAsync(_options);

        public  Task StopAsync() => _mqttClient.StartAsync(_options);

        public Task StopAsync(CancellationToken cancellationToken) => _mqttClient.StopAsync();

        public Task PublishAsync(
            string payload,
            string topic = "/client",
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
