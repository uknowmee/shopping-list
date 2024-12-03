using Microsoft.Extensions.Configuration;

namespace Shopping.List.Tests.E2E;

public static class Utils
{
    public static TConfiguration Build<TConfiguration>(IConfiguration configuration, string sectionName) where TConfiguration : new()
    {
        var options = new TConfiguration();
        var section = configuration.GetSection(sectionName);

        try
        {
            if (section.Get<TConfiguration>() == null)
            {
                return options;
            }

            section.Bind(options, binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);
        }
        catch (Exception ex)
        {
            return options;
        }

        return options;
    }
    
    public static string GetProjectRoot()
    {
        // this usually works only in console apps.
        // AppDomain.CurrentDomain.BaseDirectory will return the directory where the .dll / .exe are located
        // usually when u deploy something with IIS / Azure / Docker ect. this will not work.
        // should work for both "dotnet run" and any "IDE run" scenarios

        var directory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(directory)?.Parent?.Parent?.Parent?.FullName ?? throw new InvalidOperationException();
        return projectDirectory;
    }
}