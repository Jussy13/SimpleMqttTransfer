using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Receiving;

namespace MqttClient.Services
{
    public interface IMqttClientService : IHostedService,
                                          IMqttClientConnectedHandler,
                                          IMqttClientDisconnectedHandler,
                                          IMqttApplicationMessageReceivedHandler
    {
        Task PublishAsync(string payload, string topic = "/client", bool retainFlag = false, int qos = 1);

        Task SubscribeAsync(string topic, int qos = 1);
    }
}
