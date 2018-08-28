using Lurumad.AspNet.FileProviders.AzureStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StaticFiles
{
    public class Startup
    {
        private const string RequestPath = "/site";
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AzureOptions>(configuration.GetSection(nameof(AzureOptions)));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var fileProvider = new AzureBlobStorageFileProvider(configuration.GetSection<AzureOptions>());

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider,
                RequestPath = RequestPath
            })
            .UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = fileProvider,
                RequestPath = RequestPath
            });
        }
    }
}
