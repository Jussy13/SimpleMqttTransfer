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

            int i = 5;
            while (i >0)
            {
                mqttClient.PublishAsync($"Info from Publisher: {i}");
                Task.Delay(TimeSpan.FromSeconds(15));
                --i;
            }

            Console.ReadLine();
        }

        private static IServiceCollection CreateServiceCollection() => new Startup().ConfigureServices();
    }
}