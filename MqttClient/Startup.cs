using System;
using Database.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MqttClient.Extensions;
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
            services.AddMqttClientHostedService();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            app.UseMvc();
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
