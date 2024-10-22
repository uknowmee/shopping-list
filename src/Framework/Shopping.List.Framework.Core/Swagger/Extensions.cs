using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shopping.List.Framework.Core.Swagger;

internal static class Extensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
        => services
            .AddSwaggerGen()
            .AddEndpointsApiExplorer();

    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
    {
        if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            SwaggerBuilderExtensions
                .UseSwagger(app)
                .UseSwaggerUI();
        }

        return app;
    }
}