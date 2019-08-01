using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AuditlogService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("About to run......");
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                LoadConfigurations(config, environmentName);
                config.AddEnvironmentVariables();
                config.AddUserSecrets<Startup>();
            })
            .UseStartup<Startup>()
            .UseHealthChecks("/hc");

        public static void LoadConfigurations(IConfigurationBuilder config, string environmentName)
        {
            config
                .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.graphql.json", optional: false, reloadOnChange: true);
        }
    }
}
