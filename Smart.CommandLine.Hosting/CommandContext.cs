namespace Smart.CommandLine.Hosting;

public sealed class CommandContext
{
    public Dictionary<string, object?> Items
    {
        get
        {
            field ??= [];
            return field;
        }
    }

    public Type CommandType { get; }

    public ICommandHandler Command { get; }

    public CancellationToken CancellationToken { get; }

    public int ExitCode { get; set; }

    public CommandContext(Type commandType, ICommandHandler command, CancellationToken cancellationToken)
    {
        CommandType = commandType;
        Command = command;
        CancellationToken = cancellationToken;
    }
}
