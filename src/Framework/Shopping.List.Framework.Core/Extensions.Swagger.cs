using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Shopping.List.Framework.Core;

public static partial class Extensions
{
    private static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen()
            .AddEndpointsApiExplorer();

    private static IApplicationBuilder UseSwagger(this IApplicationBuilder app)
    {
        var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        if (env.IsDevelopment() || env.IsStaging())
        {
            SwaggerBuilderExtensions.UseSwagger(app)
                .UseSwaggerUI();
        }

        return app;
    }
}