using System;
using Database.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MqttClient.Extensions;
using MqttClient.Helpers;
using MqttClient.Services;
using MqttClient.Settings;

namespace MqttClient
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            MapConfiguration();
        }

        private void MapConfiguration()
        {
            MapBrokerHostSettings();
            MapClientSettings();
        }

        private void MapBrokerHostSettings()
        {
            BrokerHostSettings brokerHostSettings = new BrokerHostSettings();
            _configuration.GetSection(nameof(BrokerHostSettings)).Bind(brokerHostSettings);
            AppSettingsProvider.BrokerHostSettings = brokerHostSettings;
        }

        private void MapClientSettings()
        {
            ClientSettings clientSettings = new ClientSettings();
            _configuration.GetSection(nameof(ClientSettings)).Bind(clientSettings);
            AppSettingsProvider.ClientSettings = clientSettings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CalculationContext>(
                CreateCalculationContext(_configuration),
                ServiceLifetime.Singleton
            );
            services.AddSingleton<SortSourceByQueryParameterHelper<Message>>();
            services.AddControllers();
            services.AddHttpClient<HttpTransportService>(c =>
            {
                c.BaseAddress = new Uri(_configuration.GetValue<string>("uri"));
            });
            services.AddMqttClientHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Action<DbContextOptionsBuilder> CreateCalculationContext(IConfiguration configuration)
        {
            return (optionsBuilder) =>
            {
                optionsBuilder
                    .UseNpgsql(
                        configuration.GetConnectionString("db"),
                        options => options.EnableRetryOnFailure(3));
            };
        }
    }
}
