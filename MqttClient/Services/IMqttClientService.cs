using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;

namespace MqttClient.Services
{
    public interface IMqttClientService : IHostedService,
                                          IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
        Task StartAsync();

        Task StopAsync();

        Task PublishAsync(
            string payload,
            string topic = "/client",
            bool retainFlag = false,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce
        );

        Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce);
    }
}
