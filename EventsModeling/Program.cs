using System;
using EventsModeling.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EventsModeling
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServiceCollection().BuildServiceProvider();
            var executor = serviceProvider.GetRequiredService<Executor>();
            var endOfExecution = Executor.ExecutionTime + TimeSpan.FromMinutes(AppSettingsProvider.ModelTime);

            executor.Execute(endOfExecution);
        }

        private static IServiceCollection CreateServiceCollection() => Startup.ConfigureServices();
    }
}
