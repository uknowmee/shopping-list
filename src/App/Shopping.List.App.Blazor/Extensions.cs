using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Shopping.List.App.Blazor.Components.ShoppingLists;

namespace Shopping.List.App.Blazor;

public static partial class Extensions
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IShoppingUserService, ShoppingUserService>();
        return builder;
    }

    public static WebApplication UseCoreServices(this WebApplication app)
    {
        return app;
    }

    public static LoggerConfiguration ApplyConfiguration(this LoggerConfiguration configuration)
    {
        var controlLevelSwitch = new LoggingLevelSwitch();
        var consoleLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);
        var fileLevelSwitch = new LoggingLevelSwitch(LogEventLevel.Verbose);

        const string defaultSeqUrl = "http://localhost:5341";
        var seqUrl = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SEQ_HOST"))
            ? defaultSeqUrl
            : Environment.GetEnvironmentVariable("SEQ_HOST") ?? defaultSeqUrl;

        var seqApiKey = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SEQ_API_KEY"))
            ? null
            : Environment.GetEnvironmentVariable("SEQ_API_KEY");

        return configuration
            .MinimumLevel.ControlledBy(controlLevelSwitch)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .WriteTo.Async(a => a.File(
                    path: "./logs/log-.jsonl",
                    formatter: new JsonFormatter(),
                    rollingInterval: RollingInterval.Day,
                    levelSwitch: fileLevelSwitch
                )
            )
            .WriteTo.Async(a => a.Console(
                    outputTemplate: "[{Timestamp:o} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}",
                    levelSwitch: consoleLevelSwitch
                )
            )
            .WriteTo.Seq(
                serverUrl: seqUrl,
                apiKey: seqApiKey,
                controlLevelSwitch: controlLevelSwitch
            )
            .Enrich.WithProperty("Application", "Shopping.List.App.Blazor");
    }
}