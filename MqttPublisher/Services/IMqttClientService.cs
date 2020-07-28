using System.Threading.Tasks;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace MqttPublisher.Services
{
    public interface IMqttClientService : IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
        Task StartAsync();

        Task StopAsync();

        Task PublishAsync(string payload, string topic = "/publisher", bool retainFlag = false, int qos = 1);

        Task SubscribeAsync(string topic, int qos);
    }
}
