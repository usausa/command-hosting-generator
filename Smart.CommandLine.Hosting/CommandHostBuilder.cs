namespace Smart.CommandLine.Hosting;

using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1001
internal sealed class CommandHostBuilder : ICommandHostBuilder
{
    private readonly string[] args;

    private readonly ServiceCollection services = new();

    private readonly ConfigurationManager configuration;

    private readonly HostEnvironment environment;

    private readonly LoggingBuilder loggingBuilder;

    private Action<ICommandBuilder>? commandConfiguration;

    private object? serviceProviderFactory;

    private Action<object>? containerConfiguration;

    public ConfigurationManager Configuration => configuration;

    public IHostEnvironment Environment => environment;

    public IServiceCollection Services => services;

    public ILoggingBuilder Logging => loggingBuilder;

    public CommandHostBuilder(string[] args, bool useDefaults = true)
    {
        this.args = args;

        // Environment
        var contentRootPath = AppContext.BaseDirectory;
        environment = new HostEnvironment
        {
            ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty,
            EnvironmentName = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                              ?? "Production",
            ContentRootPath = contentRootPath,
            ContentRootFileProvider = new PhysicalFileProvider(contentRootPath)
        };

        // Configuration
        configuration = new ConfigurationManager();

        // Logging
        loggingBuilder = new LoggingBuilder(services);

        // TODO
        //if (useDefaults)
        //{
        //    this.UseDefaultConfiguration();
        //    this.UseDefaultLogging();
        //}
        //else
        //{
        //    services.AddLogging(builder =>
        //    {
        //        builder.AddConsole();
        //    });
        //}

        // Add basic services
        services.AddSingleton<IConfiguration>(configuration);
        services.AddSingleton<IHostEnvironment>(environment);

        services.AddSingleton<FilterPipeline>();

        services.AddOptions<CommandFilterOptions>();
    }

    public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null)
        where TContainerBuilder : notnull
    {
        serviceProviderFactory = factory;
        if (configure is not null)
        {
            containerConfiguration = obj => configure((TContainerBuilder)obj);
        }
    }

    public ICommandHostBuilder ConfigureCommands(Action<ICommandBuilder> configure)
    {
        commandConfiguration = configure;
        return this;
    }

    public ICommandHost Build()
    {
        // TODO
        throw new NotImplementedException();
    }
}
#pragma warning restore CA1001

internal sealed class HostEnvironment : IHostEnvironment
{
    public string ApplicationName { get; set; } = default!;

    public string EnvironmentName { get; set; } = default!;

    public string ContentRootPath { get; set; } = default!;

    public IFileProvider ContentRootFileProvider { get; set; } = default!;
}

internal sealed class LoggingBuilder : ILoggingBuilder
{
    public IServiceCollection Services { get; }

    public LoggingBuilder(IServiceCollection services)
    {
        Services = services;
    }
}
