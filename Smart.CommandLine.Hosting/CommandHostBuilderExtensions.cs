namespace Smart.CommandLine.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class CommandHostBuilderExtensions
{
    public static ICommandHostBuilder UseDefaults(this ICommandHostBuilder builder)
    {
        return builder
            .UseDefaultConfiguration()
            .UseDefaultLogging();
    }

    public static ICommandHostBuilder UseDefaultConfiguration(this ICommandHostBuilder builder)
    {
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddEnvironmentVariables();
        return builder;
    }

    public static ICommandHostBuilder UseDefaultLogging(this ICommandHostBuilder builder)
    {
        builder.Services.AddLogging(logging =>
        {
            // Add configuration section
            var loggingSection = builder.Configuration.GetSection("Logging");
            if (loggingSection.Exists())
            {
                logging.AddConfiguration(loggingSection);
            }
            logging.AddConsole();
        });

        return builder;
    }
}
