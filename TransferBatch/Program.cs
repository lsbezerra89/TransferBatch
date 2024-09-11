using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransferBatch.App;
using TransferBatch.App.IoC;

class Program
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        var runner = host.Services.GetRequiredService<ApplicationRunner>();
        await runner.RunAsync(args);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                var startup = new Startup();
                startup.ConfigureServices(services);
                
                services.AddTransient<ApplicationRunner>();
            });

}
