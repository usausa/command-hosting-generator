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

    public ICommand Command { get; }

    public CancellationToken CancellationToken { get; }

    public int ExitCode { get; set; }

    public CommandContext(Type commandType, ICommand command, CancellationToken cancellationToken)
    {
        CommandType = commandType;
        Command = command;
        CancellationToken = cancellationToken;
    }
}
