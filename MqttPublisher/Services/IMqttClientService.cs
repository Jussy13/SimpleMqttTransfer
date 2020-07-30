using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;

namespace MqttPublisher.Services
{
    public interface IMqttClientService : IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
        Task StartAsync();

        Task StopAsync();

        Task PublishAsync(
            string payload,
            string topic = "/publisher",
            bool retainFlag = false,
            MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce
        );

        Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtLeastOnce);
    }
}
