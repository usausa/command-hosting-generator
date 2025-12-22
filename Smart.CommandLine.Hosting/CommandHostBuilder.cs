namespace Smart.CommandLine.Hosting;

using System.CommandLine;
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
        // Execute command configuration
        var commandBuilder = new CommandBuilder(services);
        commandConfiguration?.Invoke(commandBuilder);

        // Execute filter configuration
        // TODO

        // Service provider
        IServiceProvider serviceProvider;
        if (serviceProviderFactory != null)
        {

        }
        else
        {
            serviceProvider = services.BuildServiceProvider();
        }

        //var customRootCommand = commandConfigurator.GetCustomRootCommand();
        //var rootCommand = customRootCommand ?? new RootCommand();

        // TODO
        throw new NotImplementedException();
    }
}
#pragma warning restore CA1001

//--------------------------------------------------------------------------------
// Component builders
//--------------------------------------------------------------------------------

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

//--------------------------------------------------------------------------------
// Command builders
//--------------------------------------------------------------------------------

internal sealed class CommandBuilder : ICommandBuilder
{
    private readonly IServiceCollection services;

    private Action<RootCommand>? rootCommandConfiguration;

    private RootCommand? customRootCommand;

    private readonly List<CommandDescriptor> commandRegistrations = new();

    private readonly CommandFilterOptions filterOptions = new();

    public CommandBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public ICommandBuilder ConfigureRootCommand(Action<IRootCommandBuilder> configure)
    {
        throw new NotImplementedException();
    }

    public ICommandBuilder AddCommand<TCommand>(Action<ISubCommandBuilder>? configure = null)
        where TCommand : class
    {
        return AddCommand<TCommand>(null, configure);
    }

    public ICommandBuilder AddCommand<TCommand>(Action<CommandActionBuilderContext>? builder, Action<ISubCommandBuilder>? configure = null)
        where TCommand : class
    {
        throw new NotImplementedException();
    }

    public ICommandBuilder AddGlobalFilter<TFilter>(int order = 0)
        where TFilter : class, ICommandFilter
    {
        throw new NotImplementedException();
    }

    public ICommandBuilder AddGlobalFilter(Type filterType, int order = 0)
    {
        throw new NotImplementedException();
    }

    public ICommandBuilder ConfigureFilterOptions(Action<CommandFilterOptions> configure)
    {
        configure(filterOptions);
        return this;
    }

    internal Action<RootCommand>? GetRootCommandConfiguration() => rootCommandConfiguration;

    internal RootCommand? GetCustomRootCommand() => customRootCommand;

    internal List<CommandDescriptor> GetCommandDescriptors() => commandRegistrations;

    internal CommandFilterOptions GetFilterOptions() => filterOptions;
}

internal sealed class RootCommandBuilder : IRootCommandBuilder
{
    public IRootCommandBuilder WithName(string name)
    {
        throw new NotImplementedException();
    }

    public IRootCommandBuilder WithDescription(string description)
    {
        throw new NotImplementedException();
    }

    public IRootCommandBuilder UseRootCommand(RootCommand rootCommand)
    {
        throw new NotImplementedException();
    }

    public IRootCommandBuilder Configure(Action<RootCommand> configure)
    {
        throw new NotImplementedException();
    }
}

internal sealed class SubCommandBuilder : ISubCommandBuilder
{
    private readonly IServiceCollection services;

    private readonly List<CommandDescriptor> registrations = [];

    public SubCommandBuilder(IServiceCollection services)
    {
        this.services = services;
    }

    public ISubCommandBuilder AddSubCommand<TCommand>(Action<ISubCommandBuilder>? configure = null)
        where TCommand : class
    {
        return AddSubCommand<TCommand>(null, configure);
    }

    public ISubCommandBuilder AddSubCommand<TCommand>(Action<CommandActionBuilderContext>? builder, Action<ISubCommandBuilder>? configure = null)
        where TCommand : class
    {
        if (typeof(ICommand).IsAssignableFrom(typeof(TCommand)))
        {
            services.AddTransient<TCommand>();
        }

        var registration = new CommandDescriptor(typeof(TCommand), builder);

        if (configure != null)
        {
            var subConfigurator = new SubCommandBuilder(services);
            configure(subConfigurator);
            registration.SubCommands.AddRange(subConfigurator.GetRegistrations());
        }

        registrations.Add(registration);
        return this;
    }

    internal List<CommandDescriptor> GetRegistrations() => registrations;
}
