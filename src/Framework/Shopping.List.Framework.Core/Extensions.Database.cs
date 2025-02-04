using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shopping.List.Framework.Core;

public static partial class Extensions
{
    public static WebApplicationBuilder AddDatabase<TDatabaseCtx>(this WebApplicationBuilder builder, string configurationSection) where TDatabaseCtx : DbContext
    {
        var dbOptions = ConfigurationBuildingHelper.Build<DatabaseOptions>(builder.Configuration, configurationSection);

        builder.Services.AddDbContext<TDatabaseCtx>(options =>
            {
                _ = dbOptions.Type switch
                {
                    DatabaseType.Postgres => options.UseNpgsql(dbOptions.ConnectionString),
                    _ => throw new InvalidOperationException($"Unsupported database type: {dbOptions.Type}")
                };
            }
        );

        return builder;
    }

    public static IApplicationBuilder CreateDatabase<TDatabase>(this IApplicationBuilder app) where TDatabase : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TDatabase>();
        dbContext.Database.EnsureCreated();

        return app;
    }
}

public enum DatabaseType
{
    Postgres = 0
}

public class DatabaseOptions
{
    public DatabaseType Type { get; set; } = DatabaseType.Postgres;
    public string ConnectionString { get; set; } = string.Empty;
}