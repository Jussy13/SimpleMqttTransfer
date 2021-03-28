using EventsModeling.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventsModeling
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServiceCollection().BuildServiceProvider();
            var executor = serviceProvider.GetRequiredService<Executor>();
            executor.Execute();
        }

        private static IServiceCollection CreateServiceCollection() => Startup.ConfigureServices();
    }
}
