using FluentValidation.AspNetCore;
using Lurumad.AspNet.FileProviders.AzureStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Infrastructure.Engine;
using RazorEmailTemplates.Repositories;
using RazorEmailTemplates.Services;

namespace RazorEmailTemplates
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<AzureOptions>(configuration.GetSection(nameof(AzureOptions)))
                .AddSingleton(configuration.GetSection<AzureOptions>())
                .AddMvcCore()
                .AddViews()
                .AddRazorViewEngine(engine =>
                {
                    engine.ViewLocationFormats.Clear();
                    engine.ViewLocationFormats.Add($"{{0}}/{{1}}/{{1}}{RazorViewEngine.ViewExtension}");
                    engine.ViewLocationFormats.Add($"Shared/{{0}}{RazorViewEngine.ViewExtension}");
                    engine.FileProviders.Add(new AzureBlobStorageFileProvider(configuration.GetSection<AzureOptions>()));
                })
                .AddJsonFormatters()
                .AddApiExplorer()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<TemplateCreateValidator>())
                .Services
                .AddOpenApi()
                .AddScoped<IViewToStringRenderer, RazorViewToStringRenderer>()
                .AddScoped<ITemplatesRepository, TemplatesRepository>()
                .AddScoped<ITemplatesService, TemplatesService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseAuthentication()
                .UseOpenApi()
                .UseMvc();
        }
    }
}
