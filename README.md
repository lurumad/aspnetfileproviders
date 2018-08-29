[![Build status](https://ci.appveyor.com/api/projects/status/74qic5p9f6vvjwdv?svg=true)](https://ci.appveyor.com/project/lurumad/aspnetfileproviders) [![NuGet](https://img.shields.io/nuget/v/Lurumad.AspNet.FileProviders.AzureStorage.svg)](https://www.nuget.org/packages/Lurumad.AspNet.FileProviders.AzureStorage/)

[![Build history](https://buildstats.info/appveyor/chart/lurumad/aspnetfileproviders)](https://ci.appveyor.com/project/lurumad/aspnetfileproviders/history)

# Lurumad.AspNet.FileProviders

A collection of File Providers for ASP.NET Core:

- **Azure Blob Storage File Provider** for ASP.NET Core.

  This file provider allows our ASP.NET Core application to use Azure blobs as if they would actually be files stored in server disk.

  As an example, we could use this provider to make our ASP.NET application serve blobs as static files, based in a relative request path.

  If we would have following blobs inside an Azure Blob Container:

  - Blob1.txt

  - Blob2.txt

  and we'd configure the provider with "/site" relative path, we could ask for the blobs using following url's:

      https://server/site/Blob1.txt and https://server/site/Blob2.txt

## Getting Started with Azure Storage File Provider

1. Install the Nuget Package into your ASP.NET Core application.

``` PowerShell
Install-Package Lurumad.AspNet.FileProviders.AzureStorage
```

2. Usage

An example of using AzureBlobStorageFileProvider with static files and directory browser:

```csharp
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
```

More advanced scenario of using AzureBlobStorageFileProvider with Razor View Engine as a email template system:

```csharp
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
```

You can play with the samples.
