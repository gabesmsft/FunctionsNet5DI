using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Diagnostics;

namespace FunctionsNet5DI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // #if DEBUG
            //             Debugger.Launch();
            // #endif
            var host = new HostBuilder()
                .ConfigureAppConfiguration(c =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    c.AddCommandLine(args)
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings1.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
                })
                .ConfigureFunctionsWorkerDefaults((c, b) =>
                {
                    b.UseFunctionExecutionMiddleware();
                })
                .ConfigureServices(s =>
                {
                    //Registering MyDependency1 and MyDependency2 as services so that they can be dependency-injected into a client
                    s.AddTransient<IMyDependency1, MyDependency1>(s => new MyDependency1("MyDependency1 successfully injected"));
                    s.AddTransient<IMyDependency2, MyDependency2>();
                })
                .ConfigureLogging(l =>
                {
                    l.ClearProviders();
                    l.AddApplicationInsights();
                    l.AddConsole();                  
                })
                .Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("In the Main method at " + DateTime.UtcNow);

            await host.RunAsync();
        }
    }
}

