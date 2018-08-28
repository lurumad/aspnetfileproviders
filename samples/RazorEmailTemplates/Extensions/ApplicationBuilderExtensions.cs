namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        private const string Url = "/swagger/v1/swagger.json";

        public static IApplicationBuilder UseOpenApi(this IApplicationBuilder app) =>
            app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(Url, "RazorEmailkTemplates V1");
                    c.RoutePrefix = "";
                });

    }
}
