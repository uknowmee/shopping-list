using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;
using Shopping.List.Framework.Core.Serilog;
using Shopping.List.Framework.Core.Swagger;

namespace Shopping.List.Framework.Core;

public static class Extensions
{
    public static WebApplicationBuilder AddFramework(this WebApplicationBuilder builder, Func<LoggerConfiguration, LoggerConfiguration>? updateLoggerConfiguration = null)
    {
        builder.Configuration.AddEnvironmentVariables();

        builder
            .AddLogging()
            .Services
            .AddSwagger()
            .AddLogger(updateLoggerConfiguration);

        return builder;
    }

    public static WebApplication UseFramework(this WebApplication app)
    {
        app
            .UseContextLogger()
            .UseSwagger();

        return app;
    }
}