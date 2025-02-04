using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Shopping.List.Framework.Core;

public static partial class Extensions
{
    private static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        return builder;
    }

    private static IServiceCollection AddLogger(this IServiceCollection services, Func<LoggerConfiguration, LoggerConfiguration>? updateLoggerConfiguration)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration);

        loggerConfiguration = updateLoggerConfiguration is null
            ? loggerConfiguration
            : updateLoggerConfiguration(loggerConfiguration);

        Log.Logger = loggerConfiguration.CreateLogger();

        var factory = new LoggerFactory(new ILoggerProvider[]
            {
                new SerilogLoggerProvider(Log.Logger)
            }
        );

        services.AddSingleton(factory);
        
        return services;
    }

    private static IApplicationBuilder UseContextLogger(this IApplicationBuilder app) => app.UseSerilogRequestLogging();
}