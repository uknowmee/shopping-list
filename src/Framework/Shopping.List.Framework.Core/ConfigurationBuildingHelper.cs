using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Shopping.List.Framework.Core;

public static class ConfigurationBuildingHelper
{
    public static TConfiguration Build<TConfiguration>(IConfiguration configuration, string sectionName, ILogger? logger = null) where TConfiguration : new()
    {
        logger ??= NullLogger.Instance;

        var options = new TConfiguration();
        var section = configuration.GetSection(sectionName);

        try
        {
            if (section.Get<TConfiguration>() == null)
            {
                logger.LogWarning("Failed to find {SectionName} section in configuration. Defaulting", sectionName);
                return options;
            }

            section.Bind(options, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to bind {SectionName} section with {Configuration}. Defaulting", sectionName, options);
            return options;
        }

        return options;
    }
}