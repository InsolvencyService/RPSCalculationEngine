using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog.Web;

namespace Insolvency.CalculationsEngine.Redundancy.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                (WebHostBuilderContext context, IConfigurationBuilder builder) =>
                {
                    builder
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("ConfigLookups.json", false, true)
                        .AddEnvironmentVariables();
                })
               .UseStartup<Startup>()
               .UseNLog()
               .Build();
        }
    }
}