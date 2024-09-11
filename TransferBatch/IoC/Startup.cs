using Microsoft.Extensions.DependencyInjection;
using TransferBatch.Services;
using TransferBatch.Services.Interfaces;

namespace TransferBatch.App.IoC
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<ITransferService, TransferService>();
        }
    }
}
