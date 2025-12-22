namespace Smart.CommandLine.Hosting;

using System.CommandLine;

// ReSharper disable once UnusedType.Global
#pragma warning disable CA1711
public delegate ValueTask CommandActionDelegate(
    ICommand command,
    ParseResult parseResult,
    CommandContext commandContext);
#pragma warning disable CA1711

public sealed class CommandActionBuilderContext
{
    private readonly Command command;

    public Type CommandType { get; }

    public IServiceProvider ServiceProvider { get; }

    public CommandActionDelegate? Operation { get; set; }

    public CommandActionBuilderContext(Command command, Type commandType, IServiceProvider serviceProvider)
    {
        this.command = command;
        CommandType = commandType;
        ServiceProvider = serviceProvider;
    }

    public void AddArgument(Argument argument)
    {
        command.Arguments.Add(argument);
    }
}
