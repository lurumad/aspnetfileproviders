using RazorEmailTemplates.Infrastructure.OpenApi;
using Swashbuckle.AspNetCore.Swagger;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services) =>
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.DescribeStringEnumsInCamelCase();
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(t => t.FullName);
                options.OperationFilter<FormFileOperationFilter>();
                options.SwaggerDoc("v1", new Info { Title = "RazorEmailTemplates", Version = "v1" });
            });
    }
}
