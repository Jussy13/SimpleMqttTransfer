using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MqttPublisher.Services;

namespace MqttPublisher
{
    class Program
    {
        public static void Main(string[] args)
        {
            var serviceProvider = CreateServiceCollection().BuildServiceProvider();
            var mqttClient = serviceProvider.GetService<IMqttClientService>();

            mqttClient.StartAsync();

            while (true)
            {
                mqttClient.PublishAsync("Info from Publisher");
                Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        private static IServiceCollection CreateServiceCollection() => new Startup().ConfigureServices();
    }
}