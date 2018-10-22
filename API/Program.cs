using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace API
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseUrls("http://localhost:5000")
                .UseMonitoring()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostBuilderContext, confBuilder) =>
                {
                    confBuilder
                        .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

                })
                .UseKestrel()
                .UseStartup<Startup>();

            builder.Build().Run();
        }
    }
}
