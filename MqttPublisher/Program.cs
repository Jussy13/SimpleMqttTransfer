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

            mqttClient.PublishAsync("Info from Publisher");
            // while (true)
            // {
            //     Task.Delay(TimeSpan.FromSeconds(15));
            // }

            Console.ReadLine();
        }

        private static IServiceCollection CreateServiceCollection() => new Startup().ConfigureServices();
    }
}